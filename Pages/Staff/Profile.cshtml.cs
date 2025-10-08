using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Staff
{
    public class ProfileModel : PageModel
    {
        private readonly ISystemAccountRepository _accountRepository;

        public ProfileModel(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public SystemAccount? Account { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            var accountId = (short)HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountId)!;
            Account = _accountRepository.GetAccountById(accountId);

            if (Account == null)
            {
                return RedirectToPage("/Login");
            }

            return Page();
        }

        public IActionResult OnPost(string accountName, string accountEmail, string? newPassword)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Staff)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var accountId = (short)HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountId)!;
                var account = _accountRepository.GetAccountById(accountId);

                if (account == null)
                {
                    ErrorMessage = "Account not found!";
                    return RedirectToPage();
                }

                account.AccountName = accountName;
                account.AccountEmail = accountEmail;

                if (!string.IsNullOrEmpty(newPassword))
                {
                    account.AccountPassword = newPassword;
                }

                _accountRepository.UpdateAccount(account);

                // Update session
                HttpContext.Session.SetString(SessionHelper.SessionKeyAccountName, accountName);
                HttpContext.Session.SetString(SessionHelper.SessionKeyAccountEmail, accountEmail);

                SuccessMessage = "Profile updated successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating profile: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
