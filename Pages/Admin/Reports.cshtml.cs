using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Admin
{
    public class ReportsModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepository;

        public ReportsModel(INewsArticleRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Admin)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Admin)
            {
                return RedirectToPage("/Login");
            }

            if (StartDate.HasValue && EndDate.HasValue)
            {
                NewsArticles = _newsRepository.GetNewsArticlesByDateRange(StartDate.Value, EndDate.Value);
            }

            return Page();
        }
    }
}
