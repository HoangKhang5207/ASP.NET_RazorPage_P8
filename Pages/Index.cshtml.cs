﻿using ASP.NET_RazorPage_P8.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASP.NET_RazorPage_P8.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // --------- Inject DbContext và sử dụng để tương tác với CSDL SQL Server ----------
        private readonly MyBlogContext myBlogContext;
        public IndexModel(ILogger<IndexModel> logger, MyBlogContext _myContext)
        {
            _logger = logger;
            myBlogContext = _myContext;
        }

        public void OnGet()
        {
            var posts = (from a in myBlogContext.articles
                        orderby a.Created descending
                        select a).ToList();
            ViewData["posts"] = posts;
        }
    }
}