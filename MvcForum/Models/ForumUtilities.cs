using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcForum.Models
{
    public class ForumUtilities
    {
        /// <summary>
        /// Returns a list of strings to be used for navigation bars
        /// at the bottom of paged items, such as threads and posts.
        /// </summary>
        /// <param name="itemCount">The total number of items that are paged.</param>
        /// <param name="pageNumber">The page currently displayed.</param>
        /// <returns></returns>
        public static List<String> GetNavStrings(int itemCount, int pageNumber)
        {
            List<string> result = new List<string>();
            int pageCount = itemCount / Preferences.PAGE_SIZE;

            if (itemCount % Preferences.PAGE_SIZE > 0)
            {
                pageCount++;
            }

            if (pageCount < 2)
            {
                //There is no more than 1 page, return no navigation.
                return result;
            }
            if (pageCount <= Preferences.MAX_PAGE_NAV_LINKS)
            {
                //There are no more than 10 pages, return navigation
                //to all pages. (No ellipses used between links)
                for (int i = 1; i <= pageCount; i++)
                {
                    result.Add(i.ToString());
                }
                return result;
            }

            int middle = pageCount / 2;

            //Links to show before or after the elipses
            int longSideLinkCount = Preferences.MAX_PAGE_NAV_LINKS - 2;
            //Links to the left of the current page.
            int linksLeftOfCurrent = (longSideLinkCount - 1) / 2;
            //Links to the right of the current page.
            int linksRightOfCurrent = (longSideLinkCount - 1) - linksLeftOfCurrent;
            if (pageNumber < middle)
            {                
                if (pageNumber < longSideLinkCount)
                {
                    for (int i = 1; i <= longSideLinkCount; i++)
                    {
                        result.Add(i.ToString());
                    }
                }
                else
                {
                    for (int i = pageNumber - linksLeftOfCurrent; 
                            i < pageNumber + linksRightOfCurrent; i++)
                    {
                        result.Add(i.ToString());
                    }
                }

                result.Add("...");
                result.Add((pageCount - 1).ToString());
                result.Add(pageCount.ToString());
                return result;
            }

            result.Add("1");
            result.Add("2");
            result.Add("...");

            if ((pageCount - pageNumber) < longSideLinkCount)
            {
                for (int i = pageCount - longSideLinkCount; i <= pageCount; i++)
                {
                    result.Add(i.ToString());
                }
            }
            else
            {
                for (int i = pageNumber - linksLeftOfCurrent;
                        i < pageNumber + linksRightOfCurrent; i++)
                {
                    result.Add(i.ToString());
                }
            }

            return result;

        }
    }
}