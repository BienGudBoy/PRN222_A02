using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Repositories;
using System.ComponentModel.DataAnnotations;

namespace NguyenVanBien_SE18C.NET_A02.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public LoginModel(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
            // Clear any existing session
            HttpContext.Session.Clear();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check admin credentials from appsettings.json
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (Email == adminEmail && Password == adminPassword)
            {
                // Admin login
                HttpContext.Session.SetString(SessionHelper.SessionKeyAccountEmail, Email);
                HttpContext.Session.SetString(SessionHelper.SessionKeyAccountName, "Administrator");
                HttpContext.Session.SetInt32(SessionHelper.SessionKeyAccountRole, UserRoles.Admin);
                HttpContext.Session.SetInt32(SessionHelper.SessionKeyAccountId, 0); // Admin has ID 0

                return RedirectToPage("/Admin/Reports");
            }

            // Check database for staff/lecturer
            var account = _accountRepository.GetAccountByEmailAndPassword(Email, Password);

            if (account != null)
            {
                HttpContext.Session.SetInt32(SessionHelper.SessionKeyAccountId, account.AccountID);
                HttpContext.Session.SetString(SessionHelper.SessionKeyAccountEmail, account.AccountEmail ?? "");
                HttpContext.Session.SetString(SessionHelper.SessionKeyAccountName, account.AccountName ?? "");
                HttpContext.Session.SetInt32(SessionHelper.SessionKeyAccountRole, account.AccountRole ?? 0);

                // Redirect based on role
                if (account.AccountRole == UserRoles.Staff)
                {
                    return RedirectToPage("/Staff/NewsManagement");
                }
                else if (account.AccountRole == UserRoles.Lecturer)
                {
                    return RedirectToPage("/Lecturer/NewsView");
                }
            }

            ErrorMessage = "Invalid email or password";
            return Page();
        }
    }
}
