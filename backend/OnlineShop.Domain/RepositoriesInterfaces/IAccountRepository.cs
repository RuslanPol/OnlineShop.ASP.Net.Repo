using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.RepositoriesInterfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByEmail(string email, CancellationToken cancellationToken = default);
        Task<Account?> FindByEmail(string email, CancellationToken cancellationToken = default);
        Task<bool> IsExists(string email, CancellationToken cancellationToken = default);
    }
}