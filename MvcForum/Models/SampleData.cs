using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcForum.Models
{
    public class SampleData : DropCreateDatabaseIfModelChanges<ForumEntities>
    {
        protected override void Seed(ForumEntities context)
        {
            Forum theForum = new Forum();
            List<Post> firstThreadPosts = new List<Post>();
            List<Post> pastaPosts = new List<Post>();

            List<Thread> threads = new List<Thread>();

            threads.Add( new Thread
                { 
                    ThreadStarter = "Michael",
                    Title = "The First Thread!",
                    Posts = firstThreadPosts,
                    ContainingForum = theForum                    
                }
            );
            threads.Add( new Thread
                {
                    ThreadStarter = "Wet Noodles",
                    Title = "Pasta",
                    Posts = pastaPosts,
                    ContainingForum = theForum
                }
            );

            firstThreadPosts.Add( new Post{
                    Poster = "Michael",
                    PostContent = "This is the first post on this website!",
                    PositiveRatings = 0,
                    NegativeRatings = 0,
                    SpamFlags = 0,
                    PostDate = DateTime.Parse("12/30/2011 9:49 PM"),
                    ContainingThread = threads[0]
                });

            firstThreadPosts.Add( new Post{
                    Poster = "Wet Noodles",
                    PostContent = "This is the first response to the first post!",
                    PositiveRatings = 0,
                    NegativeRatings = 0,
                    SpamFlags = 0,
                    PostDate = DateTime.Parse("12/30/2011 9:50 PM"),
                    ContainingThread = threads[0]
                }
            );
                       
            pastaPosts.Add( new Post
                   {
                    Poster = "Wet Noodles",
                    PostContent = "Let's Talk about Pasta!",
                    PositiveRatings = 0,
                    NegativeRatings = 0,
                    SpamFlags = 0,
                    PostDate = DateTime.Parse("12/30/2011 9:50 PM"),
                    ContainingThread = threads[1]
                   }
               );                      
            
            theForum.Title = "Main";
            theForum.Threads = threads;

            context.Forums.Add(theForum);
        }
    }
}