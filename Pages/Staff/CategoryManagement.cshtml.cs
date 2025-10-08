using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Staff
{
    public class CategoryManagementModel : PageModel
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManagementModel(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> Categories { get; set; } = new List<Category>();
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

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Categories = _categoryRepository.SearchCategories(searchTerm);
            }
            else
            {
                Categories = _categoryRepository.GetAllCategories();
            }

            return Page();
        }

        public IActionResult OnPostCreate(string categoryName, string categoryDescription, 
                                         short? parentCategoryId, bool isActive)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var category = new Category
                {
                    CategoryName = categoryName,
                    CategoryDescription = categoryDescription,
                    ParentCategoryID = parentCategoryId,
                    IsActive = isActive
                };

                _categoryRepository.AddCategory(category);
                SuccessMessage = "Category created successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating category: {ex.Message}";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(short categoryId, string categoryName, string categoryDescription,
                                         short? parentCategoryId, bool isActive)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var category = _categoryRepository.GetCategoryById(categoryId);
                if (category == null)
                {
                    ErrorMessage = "Category not found!";
                    return RedirectToPage();
                }

                category.CategoryName = categoryName;
                category.CategoryDescription = categoryDescription;
                category.ParentCategoryID = parentCategoryId;
                category.IsActive = isActive;

                _categoryRepository.UpdateCategory(category);
                SuccessMessage = "Category updated successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating category: {ex.Message}";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(short categoryId)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                // Check if category has news articles
                if (_categoryRepository.CategoryHasNewsArticles(categoryId))
                {
                    ErrorMessage = "Cannot delete category that has associated news articles!";
                    return RedirectToPage();
                }

                _categoryRepository.DeleteCategory(categoryId);
                SuccessMessage = "Category deleted successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting category: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
