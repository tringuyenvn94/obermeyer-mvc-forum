using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcForum.Models;

namespace MvcForum.ViewModel
{
    public class CreateThreadViewModel
    {
        public Forum TheForum { get; set; }
        public Thread TheThread { get; set; }
        public Post ThePost { get; set; }

        //public CreateThreadViewModel(Forum theForum)
        //{
        //    this.TheForum = theForum;

        //    this.TheThread = new Thread();
        //    this.TheThread.Posts = new List<Post>();

        //    this.ThePost = new Post();

        //    this.TheThread.Posts.Add(this.ThePost);
        //    this.TheForum.Threads.Add(this.TheThread);
        //}
    }
}