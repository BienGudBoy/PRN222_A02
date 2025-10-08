using Microsoft.EntityFrameworkCore;
using NguyenVanBien_SE18C.NET_A02.Data;
using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        private readonly FUNewsManagementContext _context;

        public NewsArticleRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public List<NewsArticle> GetAllNewsArticles()
        {
            return _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Include(na => na.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }

        public NewsArticle? GetNewsArticleById(string newsArticleId)
        {
            return _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Include(na => na.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefault(na => na.NewsArticleID == newsArticleId);
        }

        public void AddNewsArticle(NewsArticle newsArticle)
        {
            _context.NewsArticles.Add(newsArticle);
            _context.SaveChanges();
        }

        public void UpdateNewsArticle(NewsArticle newsArticle)
        {
            _context.NewsArticles.Update(newsArticle);
            _context.SaveChanges();
        }

        public void DeleteNewsArticle(string newsArticleId)
        {
            var newsArticle = _context.NewsArticles
                .Include(na => na.NewsTags)
                .FirstOrDefault(na => na.NewsArticleID == newsArticleId);
            
            if (newsArticle != null)
            {
                _context.NewsArticles.Remove(newsArticle);
                _context.SaveChanges();
            }
        }

        public bool NewsArticleExists(string newsArticleId)
        {
            return _context.NewsArticles.Any(na => na.NewsArticleID == newsArticleId);
        }

        public List<NewsArticle> SearchNewsArticles(string searchTerm)
        {
            return _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Include(na => na.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(na => na.NewsTitle!.Contains(searchTerm) || 
                           na.Headline.Contains(searchTerm) ||
                           na.NewsContent!.Contains(searchTerm))
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }

        public List<NewsArticle> GetActiveNewsArticles()
        {
            return _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Include(na => na.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(na => na.NewsStatus == true)
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }

        public List<NewsArticle> GetNewsArticlesByCreator(short createdById)
        {
            return _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Include(na => na.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(na => na.CreatedByID == createdById)
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }

        public List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.NewsArticles
                .Include(na => na.Category)
                .Include(na => na.CreatedBy)
                .Include(na => na.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(na => na.CreatedDate >= startDate && na.CreatedDate <= endDate)
                .OrderByDescending(na => na.CreatedDate)
                .ToList();
        }
    }
}
