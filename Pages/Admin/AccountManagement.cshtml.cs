using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NguyenVanBien_SE18C.NET_A02.Helpers;
using NguyenVanBien_SE18C.NET_A02.Models;
using NguyenVanBien_SE18C.NET_A02.Repositories;

namespace NguyenVanBien_SE18C.NET_A02.Pages.Admin
{
    public class AccountManagementModel : PageModel
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountManagementModel(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public List<SystemAccount> Accounts { get; set; } = new List<SystemAccount>();
        public string? SearchTerm { get; set; }

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public IActionResult OnGet(string? searchTerm)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Admin)
            {
                return RedirectToPage("/Login");
            }

            SearchTerm = searchTerm;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                Accounts = _accountRepository.SearchAccounts(searchTerm);
            }
            else
            {
                Accounts = _accountRepository.GetAllAccounts();
            }

            return Page();
        }

        public IActionResult OnPostCreate(short accountId, string accountName, string accountEmail, string accountPassword, int accountRole)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Admin)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                if (_accountRepository.AccountExists(accountId))
                {
                    ErrorMessage = "Account ID already exists!";
                    return RedirectToPage();
                }

                var account = new SystemAccount
                {
                    AccountID = accountId,
                    AccountName = accountName,
                    AccountEmail = accountEmail,
                    AccountPassword = accountPassword,
                    AccountRole = accountRole
                };

                _accountRepository.AddAccount(account);
                SuccessMessage = "Account created successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error creating account: {ex.Message}";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostUpdate(short accountId, string accountName, string accountEmail, string? accountPassword, int accountRole)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Admin)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                var account = _accountRepository.GetAccountById(accountId);
                if (account == null)
                {
                    ErrorMessage = "Account not found!";
                    return RedirectToPage();
                }

                account.AccountName = accountName;
                account.AccountEmail = accountEmail;
                account.AccountRole = accountRole;

                // Only update password if a new one is provided
                if (!string.IsNullOrWhiteSpace(accountPassword))
                {
                    account.AccountPassword = accountPassword;
                }

                _accountRepository.UpdateAccount(account);
                SuccessMessage = "Account updated successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating account: {ex.Message}";
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(short accountId)
        {
            var role = HttpContext.Session.GetInt32(SessionHelper.SessionKeyAccountRole);
            if (role != UserRoles.Admin)
            {
                return RedirectToPage("/Login");
            }

            try
            {
                _accountRepository.DeleteAccount(accountId);
                SuccessMessage = "Account deleted successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error deleting account: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
