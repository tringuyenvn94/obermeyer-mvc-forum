using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace MvcForum.Models
{
    public class UserProfile : ProfileBase
    {
        static public UserProfile CurrentUser
        {
            get
            {
                return (UserProfile)
                       (ProfileBase.Create(Membership.GetUser().UserName));
            }
        }
         
        
        public List<int> PostsVotedFor
        {
            get { return ((List<int>)(base["PostsVotedFor"])); }
            set { base["PostsVotedFor"] = value; Save(); }
        }

        public string FullName
        {
            get { return ((string)(base["FullName"])); }
            set { base["FullName"] = value; Save(); }
        }

        public int Age
        {
            get { return ((int)(base["Age"])); }
            set { base["Age"] = value; Save(); }
        }

        [DataType(DataType.MultilineText)]
        public string About
        {
            get { return ((string)(base["About"])); }
            set { base["About"] = value; Save(); }
        }

        public int PageSize
        {
            get { return ((int)(base["PageSize"])); }
            set { base["PageSize"] = value; Save(); }
        }


    }
}