using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcForum.Models;

namespace MvcForum.ViewModel
{
    public class ThreadListViewModel : IPagedViewModel
    {
        public Forum Forum { get; set; }
        public List<Thread> Threads { get; set; }
        public List<Thread> StickyThreads { get; set; }
        public int PageNumber { get; set; }

        public List<string> GetPageNavStrings()
        {
            return ForumUtilities.GetNavStrings(Forum.Threads.Count, PageNumber);
        }

        public int GetId()
        {
            return this.Forum.ForumId;
        }
    }
}