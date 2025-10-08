using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetAllNewsArticles();
        NewsArticle? GetNewsArticleById(string newsArticleId);
        void AddNewsArticle(NewsArticle newsArticle);
        void UpdateNewsArticle(NewsArticle newsArticle);
        void DeleteNewsArticle(string newsArticleId);
        bool NewsArticleExists(string newsArticleId);
        List<NewsArticle> SearchNewsArticles(string searchTerm);
        List<NewsArticle> GetActiveNewsArticles();
        List<NewsArticle> GetNewsArticlesByCreator(short createdById);
        List<NewsArticle> GetNewsArticlesByDateRange(DateTime startDate, DateTime endDate);
    }
}
