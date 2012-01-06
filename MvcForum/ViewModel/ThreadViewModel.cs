using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcForum.Models;
using System.Web.Mvc.Ajax;

namespace MvcForum.ViewModel
{
    public class ThreadViewModel : IPagedViewModel
    {
        #region Properties
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        /// <summary>
        /// The prefix used to specify the id attribute
        /// of a post.
        /// </summary>
        public const string DOM_POST_PREFIX_ID = "post_";

        /// <summary>
        /// Returns the id attribute value for the given
        /// post as a string.
        /// </summary>
        /// <param name="aPost">The post to get the DOM id attribute of</param>
        /// <returns>The DOM element id attribute value.</returns>
        public static string GetDomPostId(Post aPost)
        {
            return DOM_POST_PREFIX_ID + aPost.PostId;
        }
        /// <summary>
        /// Gets or sets the thread context of this ViewModel.
        /// </summary>
        public Thread Thread { get; set; }
        
       
        /// <summary>
        /// Gets or sets the post that needs to be editted.
        /// </summary>
        public Post Post { get; set; }
        #endregion

        /// <summary>
        /// Gets the Posts on the current page.
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public List<Post> GetPagePosts()
        {
            List<Post> pagePosts =
                this.Thread.Posts.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList();
            return pagePosts;
        }

        public int GetId()
        {
            return this.Thread.ThreadId;
        }
        
        public bool OwnsPost(Post thePost)
        {
            return HttpContext.Current.User.Identity.Name.Equals(thePost.Poster);
        }

        /// <summary>
        /// Returns true if there is a post to edit (edit mode).
        /// Returns false if there is no post to edit (create mode).
        /// </summary>
        /// <returns></returns>
        public bool IsEditMode()
        {
            return Post != null;
        }

        /// <summary>
        /// Returns the action string based on the context of
        /// this ThreadViewModel. If this ViewModel represents
        /// a state in which the user is editting, it returns
        /// "UpdatePost", otherwise, it returns "CreatePost".
        /// </summary>
        /// <returns>String: "UpdatePost" or "CreatePost"</returns>
        public string GetEditModeAction()
        {
            if (IsEditMode())
            {
                return "UpdatePost";
            }
            else
                return "CreatePost";

        }

        /// <summary>
        /// Returns the parameters necessary to call the action
        /// method based on the context of this viewmodel.
        /// </summary>
        /// <returns></returns>
        public object GetActionParameters()
        {
            if (IsEditMode())
            {
                return new
                    {
                        postId = Post.PostId,
                        threadId = Thread.ThreadId
                    };
            }
            else
            {
                return new
                    {
                        threadId = Thread.ThreadId,
                        thePost = this.Post
                    };
            }
           
        }

        /// <summary>
        /// Returns the AjaxOptions associated with the mode
        /// of this ViewModel.
        /// </summary>
        /// <returns></returns>
        public AjaxOptions GetAjaxOptions()
        {
            AjaxOptions options = new AjaxOptions();
            options.HttpMethod = "POST";
            options.OnSuccess = "ClearTextBox"; 

            if (IsEditMode())
            {
                options.UpdateTargetId = GetDomPostId(this.Post);
                              
            }
            else
            {
                options.UpdateTargetId = "posts";                
            }

            return options;
        }

        /// <summary>
        /// Returns a list of strings to be used for
        /// page navigation.
        /// </summary>
        /// <returns></returns>
        public List<string> GetPageNavStrings()
        {
            return ForumUtilities.GetNavStrings(Thread.Posts.Count, this.PageNumber);
        }
    }
}