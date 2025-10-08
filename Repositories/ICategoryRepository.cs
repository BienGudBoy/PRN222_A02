using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAllCategories();
        Category? GetCategoryById(short categoryId);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(short categoryId);
        bool CategoryExists(short categoryId);
        List<Category> SearchCategories(string searchTerm);
        bool CategoryHasNewsArticles(short categoryId);
        List<Category> GetActiveCategories();
    }
}
