using System;
namespace MvcForum.ViewModel
{
    public interface IPagedViewModel
    {
        System.Collections.Generic.List<string> GetPageNavStrings();
        int PageNumber { get; set; }
        int GetId();
    }
}
