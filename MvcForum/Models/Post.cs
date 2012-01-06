using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace MvcForum.Models
{    
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        
        public virtual Thread ContainingThread { get; set; }

        [Required(ErrorMessage = "You must be logged in to make a post.")]
        public string Poster { get; set; }

        [Required(ErrorMessage = "You must enter a response in order to post."),
         DataType(DataType.MultilineText)]
        public string PostContent { get; set; }

        [Required]
        public int PositiveRatings { get; set; }

        [Required]
        public int NegativeRatings { get; set; }

        [Required]
        public int SpamFlags { get; set; }

        [Required]
        public DateTime PostDate { get; set; }
    }
}