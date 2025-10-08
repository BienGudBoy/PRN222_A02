using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Lecturer
{
    public class NewsViewModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepository;

        public NewsViewModel(INewsArticleRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public string? SearchTerm { get; set; }

        public IActionResult OnGet(string? searchTerm)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Lecturer)
            {
                return RedirectToPage("/Login");
            }

            SearchTerm = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                NewsArticles = _newsRepository.SearchNewsArticles(searchTerm)
                    .Where(na => na.NewsStatus == true)
                    .ToList();
            }
            else
            {
                NewsArticles = _newsRepository.GetActiveNewsArticles();
            }

            return Page();
        }
    }
}
