using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcForum.Models
{
    public class Thread
    {
        [Key]
        public int ThreadId { get; set; }
       
        public virtual Forum ContainingForum { get; set; }
        public string ThreadStarter { get; set; }
        [Required(ErrorMessage = "A Thread Title is required.")]
        public string Title { get; set; }
        
        public bool IsSticky { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}