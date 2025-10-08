using Microsoft.EntityFrameworkCore;
using NguyenVanBien_SE18C.NET_A02.Data;
using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FUNewsManagementContext _context;

        public CategoryRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public List<Category> GetAllCategories()
        {
            return _context.Categories
                .Include(c => c.ParentCategory)
                .ToList();
        }

        public Category? GetCategoryById(short categoryId)
        {
            return _context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefault(c => c.CategoryID == categoryId);
        }

        public void AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(short categoryId)
        {
            var category = GetCategoryById(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public bool CategoryExists(short categoryId)
        {
            return _context.Categories.Any(c => c.CategoryID == categoryId);
        }

        public List<Category> SearchCategories(string searchTerm)
        {
            return _context.Categories
                .Include(c => c.ParentCategory)
                .Where(c => c.CategoryName.Contains(searchTerm) || 
                           c.CategoryDescription.Contains(searchTerm))
                .ToList();
        }

        public bool CategoryHasNewsArticles(short categoryId)
        {
            return _context.NewsArticles.Any(na => na.CategoryID == categoryId);
        }

        public List<Category> GetActiveCategories()
        {
            return _context.Categories
                .Where(c => c.IsActive == true)
                .ToList();
        }
    }
}
