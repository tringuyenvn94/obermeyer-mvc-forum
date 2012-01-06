using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcForum.Models;
using MvcForum.ViewModel;

namespace MvcForum.Controllers
{
    public class PostController : Controller
    {
        ForumEntities db = new ForumEntities();
        private int pageNumber = 1;

        //
        // GET: /Thread/5

        /// <summary>
        /// Returns the view that lists the posts in the given
        /// thread.
        /// </summary>
        /// <param name="id">The thread to show the posts of</param>
        /// <returns></returns>
        public ActionResult Index(int id, int? page)
        {
            this.pageNumber = page ?? 1;
            this.pageNumber = (this.pageNumber < 1)? 1 : this.pageNumber;

            Thread thread = db.Threads.SingleOrDefault(t => t.ThreadId == id);

            ThreadViewModel tvm = new ThreadViewModel
            {
                Thread = thread,
                PageNumber = this.pageNumber,
                PageSize = Preferences.PAGE_SIZE
            };

            return View(tvm);
        }
        
        //
        // AJAX /Thread/DeletePost?postId=postId&threadId=threadId
        [HttpPost]
        [Authorize]
        public ActionResult DeletePost(int postId, int threadId)
        {
            Post thePost = db.Posts.SingleOrDefault(p => p.PostId == postId);
            Thread thread = db.Threads.SingleOrDefault(t => t.ThreadId == threadId);
                        
            db.Posts.Remove(thePost);
            db.SaveChanges();

           
            ThreadViewModel tvm = new ThreadViewModel
            {
                Thread = thread,
                PageNumber = this.pageNumber,
                PageSize = Preferences.PAGE_SIZE
            };

            return PartialView("Posts", tvm);
        }

        //
        // AJAX /Thread/EditPost?postId&threadId
        [HttpPost]
        [Authorize]
        public ActionResult EditPost(int postId, int threadId)
        {
            Thread theThread = db.Threads.Single(t => t.ThreadId == threadId);
            Post thePost = db.Posts.Single(p => p.PostId == postId);

            ThreadViewModel tvm = new ThreadViewModel
            {
                Thread = theThread,
                PageNumber = this.pageNumber,
                PageSize = Preferences.PAGE_SIZE,
                Post = thePost
            };
            return PartialView("PostEditor", tvm);
        }

        //
        // AJAX /Thread/UpdatePost?postId&threadId
        [HttpPost]
        [Authorize]
        public ActionResult UpdatePost(int postId, int threadId, FormCollection form)
        {

            Post thePost = db.Posts.Single(p => p.PostId == postId);
            
            try
            {               
                UpdateModel(thePost, "Post");
                db.SaveChanges();
                //Success: return uneditable post.
                return PartialView("SinglePost", thePost);
            }
            catch
            {
                //Fail: return editable post.
                Thread theThread = db.Threads.Single(t => t.ThreadId == threadId);
                ThreadViewModel tvm = new ThreadViewModel
                {
                    Thread = theThread,
                    Post = thePost,
                    PageNumber = this.pageNumber,
                    PageSize = Preferences.PAGE_SIZE
                };
                return PartialView("PostEditor", tvm);
            }
        }
        
        //
        // AJAX /Thread/CreatePost
        [HttpPost, Authorize]
        public ActionResult CreatePost(int threadId, FormCollection form)
        {
            Thread theThread = db.Threads.Single(t => t.ThreadId == threadId);

            Post thePost = new Post();
            thePost.ContainingThread = theThread;
            thePost.Poster = User.Identity.Name;            
            thePost.PostDate = DateTime.Now;

            /*Post thePost = new Post
            {
                PostContent = postContent,
                Poster = User.Identity.Name,
                NegativeRatings = 0,
                PositiveRatings = 0,
                SpamFlags = 0,
                PostDate = DateTime.Now
            };*/

            ThreadViewModel tvm = new ThreadViewModel 
            { 
                Thread = theThread
            };
            try
            {
                this.UpdateModel<Post>(thePost, "Post");
                theThread.Posts.Add(thePost);
                db.SaveChanges();

                int lastPageSize = theThread.Posts.Count % Preferences.PAGE_SIZE;
                int lastPage = theThread.Posts.Count / Preferences.PAGE_SIZE;

                lastPage = (lastPageSize == 0) ? lastPage : lastPage + 1;

                tvm.PageSize = Preferences.PAGE_SIZE;
                tvm.PageNumber = lastPage;

                return PartialView("Posts", tvm);
            }
            catch
            {
                tvm.PageSize = Preferences.PAGE_SIZE;
                tvm.PageNumber = this.pageNumber;
                return PartialView("Posts", tvm);
            }
        }

        //
        //AJAX
        [HttpPost, Authorize]
        public ActionResult RatePost(int id, bool rateUp)
        {
            Post thePost = db.Posts.SingleOrDefault(p => p.PostId == id);
            if (UserProfile.CurrentUser.PostsVotedFor.Contains(id))
            {
                return Content("You have already rated this post."
                    + " +" + thePost.PositiveRatings.ToString()
                    + " | -"  + thePost.NegativeRatings.ToString());
            }
            else
            {
                if (rateUp)
                    thePost.PositiveRatings++;
                else
                    thePost.NegativeRatings++;
                db.SaveChanges();               
                List<int> ratedPosts = UserProfile.CurrentUser.PostsVotedFor;
                ratedPosts.Add(id);
                UserProfile.CurrentUser.PostsVotedFor = ratedPosts;
                return PartialView("VoteModule", thePost);
            }
        }
       
    }
}
