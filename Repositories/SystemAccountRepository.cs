using Microsoft.EntityFrameworkCore;
using NguyenVanBien_SE18C.NET_A02.Data;
using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FUNewsManagementContext _context;

        public SystemAccountRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public SystemAccount? GetAccountByEmailAndPassword(string email, string password)
        {
            return _context.SystemAccounts
                .FirstOrDefault(a => a.AccountEmail == email && a.AccountPassword == password);
        }

        public SystemAccount? GetAccountById(short accountId)
        {
            return _context.SystemAccounts.Find(accountId);
        }

        public List<SystemAccount> GetAllAccounts()
        {
            return _context.SystemAccounts.ToList();
        }

        public void AddAccount(SystemAccount account)
        {
            _context.SystemAccounts.Add(account);
            _context.SaveChanges();
        }

        public void UpdateAccount(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            _context.SaveChanges();
        }

        public void DeleteAccount(short accountId)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                _context.SystemAccounts.Remove(account);
                _context.SaveChanges();
            }
        }

        public bool AccountExists(short accountId)
        {
            return _context.SystemAccounts.Any(a => a.AccountID == accountId);
        }

        public List<SystemAccount> SearchAccounts(string searchTerm)
        {
            return _context.SystemAccounts
                .Where(a => a.AccountName!.Contains(searchTerm) || 
                           a.AccountEmail!.Contains(searchTerm))
                .ToList();
        }
    }
}
