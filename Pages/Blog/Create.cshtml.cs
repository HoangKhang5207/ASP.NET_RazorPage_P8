using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ASP.NET_RazorPage_P8.Models;

namespace ASP.NET_RazorPage_P8.Pages_Blog
{
    public class CreateModel : PageModel
    {
        private readonly ASP.NET_RazorPage_P8.Models.MyBlogContext _context;

        public CreateModel(ASP.NET_RazorPage_P8.Models.MyBlogContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        // ----------- Chức năng Create tạo bài viết mới ----------
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.articles == null || Article == null)
            {
                return Page();
            }

            _context.Add(Article);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
