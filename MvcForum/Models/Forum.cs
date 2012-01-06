using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcForum.Models
{
    public class Forum
    {
        [Required(ErrorMessage = "The Forum Title is required.")]
        public string Title { get; set; }
        [Key]
        public int ForumId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        public virtual List<Thread> Threads { get; set; }
    }
}