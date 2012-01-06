using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcForum.Models;
using MvcForum.ViewModel;

namespace MvcForum.Controllers
{
    public class ThreadController : Controller
    {
        ForumEntities db = new ForumEntities();
        private int pageNumber;

        //
        // GET: Thread/Index/5

        /// <summary>
        /// Returns the view that contains the list of threads
        /// associated with the forum with the given id.
        /// </summary>
        /// <param name="id">The id of the forum</param>
        /// <returns>View of the list of threads.</returns>
        public ActionResult Index(int id, int? page)
        {
            pageNumber = page ?? 1;
            pageNumber = (pageNumber < 1) ? 1 : pageNumber;

            Forum forum = db.Forums.SingleOrDefault(f => f.ForumId == id);
            List<Thread> threads = GetNonStickyThreads(forum);

            List<Thread> stickyThreads = (from t in forum.Threads
                                         where t.IsSticky == true
                                         select t).ToList();

            ThreadListViewModel theModel = new ThreadListViewModel
            {
                Forum = forum,
                Threads = threads,
                StickyThreads = stickyThreads,
                PageNumber = pageNumber
            };
            return View(theModel);
        }
        
        //
        // GET: Thread/Create
        [Authorize]
        public ActionResult Create(int forumId)
        {
            Forum theForum = db.Forums.Single(f => f.ForumId == forumId);
            CreateThreadViewModel ctvm = new CreateThreadViewModel 
            {
                TheForum = theForum
            }; 
            
            return View(ctvm);            
        }

        //
        // POST: Thread/Create
        [HttpPost, Authorize]
        public ActionResult Create(int forumId, FormCollection form)
        {
           
            Forum forumToUpdate = db.Forums.Single(
                f => f.ForumId == forumId);

            Post newPost = new Post
            {                
                Poster = User.Identity.Name,                
                PostDate = DateTime.Now
            };

            Thread newThread = new Thread 
            {
                ThreadStarter = User.Identity.Name,
                Posts = new List<Post>{ newPost }
            };

            newPost.ContainingThread = newThread;
            newThread.ContainingForum = forumToUpdate;
            forumToUpdate.Threads.Add(newThread);            

            try
            {
                UpdateModel(newPost, "ThePost");
                UpdateModel(newThread, "TheThread");
                db.SaveChanges();
                return RedirectToAction("Index", new { id = forumToUpdate.ForumId });
            }
            catch
            {
                return View(new CreateThreadViewModel { TheForum = forumToUpdate });
            }
        }

        //
        // GET: Thread/Delete/id
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Delete(int threadId, int forumId)
        {
            Thread theThread = db.Threads.SingleOrDefault(t => t.ThreadId == threadId);
            Forum theForum = db.Forums.SingleOrDefault(f => f.ForumId == forumId);
            CreateThreadViewModel viewModel = new CreateThreadViewModel 
            {
                TheThread = theThread,
                TheForum = theForum
            };
            return View(viewModel);
        }

        //
        // POST
        [HttpPost]
        [Authorize(Roles = "Administrator, Moderator")]
        public ActionResult Delete(int threadId)
        {
            try
            {
                Thread theThread = db.Threads.SingleOrDefault(t => t.ThreadId == threadId);
                Forum containingForum = theThread.ContainingForum;
                containingForum.Threads.Remove(theThread);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = containingForum.ForumId } );
            }
            catch
            {
                return View("Error");
            }

            
        }

        //
        // AJAX
        [HttpPost, Authorize(Roles = "Administrator, Moderator")]
        public ActionResult ToggleSticky(int id)
        {
            Thread theThread = db.Threads.Single(t => t.ThreadId == id);
            theThread.IsSticky = !theThread.IsSticky;
            db.SaveChanges();

            Forum forum = theThread.ContainingForum;
            List<Thread> threads = GetNonStickyThreads(forum);
            List<Thread> stickyThreads = (from t in forum.Threads
                                          where t.IsSticky == true
                                          select t).ToList();

            ThreadListViewModel theModel = new ThreadListViewModel
            {
                StickyThreads = stickyThreads,
                Forum = forum,
                Threads = threads,
                PageNumber = this.pageNumber
            };
            
            return PartialView("ThreadView", theModel);
        }

        private List<Thread> GetNonStickyThreads(Forum forum)
        {
            forum.Threads = forum.Threads.OrderByDescending
                                         (t => t.Posts[t.Posts.Count - 1].PostDate)
                                         .ToList();
            var nonStickyThreads = from t in forum.Threads
                                   where t.IsSticky == false
                                   select t;
            List<Thread> threads = nonStickyThreads.Skip((pageNumber - 1) * Preferences.PAGE_SIZE)
                                                .Take(Preferences.PAGE_SIZE).ToList();
            return threads;
        }        
    }
}
