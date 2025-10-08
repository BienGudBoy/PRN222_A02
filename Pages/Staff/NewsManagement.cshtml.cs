using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Hubs;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Staff
{
    public class NewsManagementModel : PageModel
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IHubContext<NewsHub> _hubContext;

        public NewsManagementModel(
            INewsArticleRepository newsRepository,
            ICategoryRepository categoryRepository,
            ITagRepository tagRepository,
            IHubContext<NewsHub> hubContext)
        {
            _newsRepository = newsRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _hubContext = hubContext;
        }

        public List<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Tag> Tags { get; set; } = new List<Tag>();
        public string? SearchTerm { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet(string? searchTerm)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            SearchTerm = searchTerm;
            LoadData();

            return Page();
        }

        private void LoadData()
        {
            var accountId = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountId);

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                NewsArticles = _newsRepository.SearchNewsArticles(SearchTerm);
            }
            else
            {
                NewsArticles = _newsRepository.GetAllNewsArticles();
            }

            Categories = _categoryRepository.GetActiveCategories();
            Tags = _tagRepository.GetAllTags();
        }

        public async Task<IActionResult> OnPostCreateAsync(string newsArticleId, string newsTitle, string headline,
            string newsContent, string? newsSource, short categoryId, bool newsStatus, int[] selectedTags)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                if (_newsRepository.NewsArticleExists(newsArticleId))
                {
                    ErrorMessage = "News Article ID already exists!";
                    return RedirectToPage();
                }

                var accountId = (short)HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountId)!;

                var newsArticle = new NewsArticle
                {
                    NewsArticleID = newsArticleId,
                    NewsTitle = newsTitle,
                    Headline = headline,
                    NewsContent = newsContent,
                    NewsSource = newsSource,
                    CategoryID = categoryId,
                    NewsStatus = newsStatus,
                    CreatedByID = accountId,
                    UpdatedByID = accountId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                _newsRepository.AddNewsArticle(newsArticle);

                // Add tags
                if (selectedTags != null && selectedTags.Length > 0)
                {
                    foreach (var tagId in selectedTags)
                    {
                        newsArticle.NewsTags.Add(new NewsTag
                        {
                            NewsArticleID = newsArticleId,
                            TagID = tagId
                        });
                    }
                    _newsRepository.UpdateNewsArticle(newsArticle);
                }

                // Notify all clients via SignalR
                await _hubContext.Clients.All.SendAsync("NewsCreated", newsArticleId, newsTitle);

                SuccessMessage = "News article created successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating news article: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync(string newsArticleId, string newsTitle, string headline,
            string newsContent, string? newsSource, short categoryId, bool newsStatus, int[] selectedTags)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var newsArticle = _newsRepository.GetNewsArticleById(newsArticleId);
                if (newsArticle == null)
                {
                    ErrorMessage = "News article not found!";
                    return RedirectToPage();
                }

                var accountId = (short)HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountId)!;

                newsArticle.NewsTitle = newsTitle;
                newsArticle.Headline = headline;
                newsArticle.NewsContent = newsContent;
                newsArticle.NewsSource = newsSource;
                newsArticle.CategoryID = categoryId;
                newsArticle.NewsStatus = newsStatus;
                newsArticle.UpdatedByID = accountId;
                newsArticle.ModifiedDate = DateTime.Now;

                // Update tags
                newsArticle.NewsTags.Clear();
                if (selectedTags != null && selectedTags.Length > 0)
                {
                    foreach (var tagId in selectedTags)
                    {
                        newsArticle.NewsTags.Add(new NewsTag
                        {
                            NewsArticleID = newsArticleId,
                            TagID = tagId
                        });
                    }
                }

                _newsRepository.UpdateNewsArticle(newsArticle);

                // Notify all clients via SignalR
                await _hubContext.Clients.All.SendAsync("NewsModified", newsArticleId, newsTitle);

                SuccessMessage = "News article updated successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating news article: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string newsArticleId)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                _newsRepository.DeleteNewsArticle(newsArticleId);

                // Notify all clients via SignalR
                await _hubContext.Clients.All.SendAsync("NewsDeleted", newsArticleId);

                SuccessMessage = "News article deleted successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting news article: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
