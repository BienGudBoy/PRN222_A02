using NguyenVanBien_SE18C.NET_A02.Models;

namespace NguyenVanBien_SE18C.NET_A02.Repositories
{
    public interface ISystemAccountRepository
    {
        SystemAccount? GetAccountByEmailAndPassword(string email, string password);
        SystemAccount? GetAccountById(short accountId);
        List<SystemAccount> GetAllAccounts();
        void AddAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        void DeleteAccount(short accountId);
        bool AccountExists(short accountId);
        List<SystemAccount> SearchAccounts(string searchTerm);
    }
}
