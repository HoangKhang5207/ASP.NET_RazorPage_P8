using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ASP.NET_RazorPage_P8.Models;
using Microsoft.AspNetCore.Authorization;

namespace ASP.NET_RazorPage_P8.Pages_Blog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ASP.NET_RazorPage_P8.Models.MyBlogContext _context;

        public IndexModel(ASP.NET_RazorPage_P8.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        // ----------- Xây dựng chức năng paging -----------
        public const int ITEMS_PER_PAGE = 5;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }


        public async Task OnGetAsync(string SearchString)
        {
            if (_context.articles != null)
            {
                //Article = await _context.articles.ToListAsync();

                int totalArticle = await _context.articles.CountAsync();

                countPages = (int)Math.Ceiling((double)totalArticle / ITEMS_PER_PAGE);

                if (currentPage < 1)
                    currentPage = 1;
                if (currentPage > countPages)
                    currentPage = countPages;

                var qr = (from a in _context.articles
                         orderby a.Created descending
                         select a).Skip((currentPage - 1) * ITEMS_PER_PAGE).Take(5);

                if (!string.IsNullOrEmpty(SearchString))
                {
                    Article = await qr.Where(a => a.Title.Contains(SearchString)).ToListAsync();
                }
                else
                {
                    Article = await qr.ToListAsync();
                }    

            }
        }
    }
}
