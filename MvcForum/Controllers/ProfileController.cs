using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcForum.Models;
using System.Web.Security;
using System.Web.Profile;

namespace MvcForum.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/

        public ActionResult Index(string userName)
        {

            return View(GetUserProfile(userName));
        }

        private static UserProfile GetUserProfile(string userName)
        {
            return (UserProfile)
                                   (ProfileBase.Create(Membership.GetUser(userName).UserName));
        }
        //
        // GET: Profile/Edit?userName="name"
        [Authorize]
        public ActionResult Edit(string userName)
        {
            if (User.Identity.Name != userName)
            {
                return RedirectToAction("Index", new { userName = userName });
            }
            else
            {
                return View(UserProfile.CurrentUser);
            }
        }

        //
        // POST:
        [Authorize, HttpPost]
        public ActionResult Edit(string user, FormCollection form)
        {
            UserProfile theProfile = GetUserProfile(user);

            try
            {
                UpdateModel(theProfile);
                return RedirectToAction("Index", new { userName = user });
            }
            catch
            {
                return View(theProfile);
            }
            
        }
    }
}
