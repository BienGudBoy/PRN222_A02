using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Staff
{
    public class MyNewsModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepository;

        public MyNewsModel(INewsArticleRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsArticle> MyNewsArticles { get; set; } = new List<NewsArticle>();

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            var accountId = (short)HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountId)!;
            MyNewsArticles = _newsRepository.GetNewsArticlesByCreator(accountId);

            return Page();
        }
    }
}
