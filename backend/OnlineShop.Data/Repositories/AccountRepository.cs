using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;

namespace OnlineShop.Data.Repositories
{
    public class AccountRepository : EfRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            if (appDbContext == null) throw new ArgumentNullException(nameof(appDbContext));
        }

        public Task<Account> GetByEmail(string email, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return Entities.SingleAsync(it => it.Email == email, cancellationToken);
        }

        public Task<Account?> FindByEmail(string email, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return Entities.FirstOrDefaultAsync(it => it.Email == email, cancellationToken);
        }


        public async Task<bool> IsExists(string email, CancellationToken cancellationToken)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            return await Entities.AnyAsync(it => it.Email == email, cancellationToken);
        }
    }
}