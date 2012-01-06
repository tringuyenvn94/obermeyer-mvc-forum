using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcForum.Models;

namespace MvcForum.Controllers
{
    public class ForumController : Controller
    {
        ForumEntities db = new ForumEntities();
        //
        // GET: /Forum/

        public ActionResult Index()
        {            
            return View(db.Forums.ToList());
        }

        //
        // GET: /Forum/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View(new Forum());
        }

        //
        // POST: /Forum/Create
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult Create(Forum theForum)
        {
            try
            {
                UpdateModel(theForum);
                db.Forums.Add(theForum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(theForum);
            }
        }

        //
        // AJAX: /Forum/Delete
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            try
            {
                Forum theForum = db.Forums.SingleOrDefault(f => f.ForumId == id);
                db.Forums.Remove(theForum);
                db.SaveChanges();
                return PartialView("ForumList", db.Forums);
            }
            catch
            {
                return PartialView("Error");
            }
        }

        //
        // GET: Forum/Edit/id
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            Forum theForum = db.Forums.SingleOrDefault(f => f.ForumId == id);

            return View(theForum);
        }

        //
        // POST: Forum/Edit/id
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult Edit(Forum theForum)
        {
            Forum updatedForum = 
                db.Forums.SingleOrDefault(f => f.ForumId == theForum.ForumId);
            try
            {
                UpdateModel(updatedForum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(theForum);
            }
        }

    }    
}
