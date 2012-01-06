using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcForum.Models
{
    public class Preferences
    {
        public int PAGE_SIZE
        {
            get
            {
                if (UserProfile.CurrentUser != null)
                {
                    return UserProfile.CurrentUser.PageSize;
                }
                else
                {
                    return 5;
                }
            }
        }
        public const int MAX_PAGE_NAV_LINKS = 10;
    }
}