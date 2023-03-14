using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;

namespace OnlineShop.Data.Repositories;

public class CartRepository:EfRepository<Cart>, ICartRepository
{
    public CartRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        if (appDbContext == null) throw new ArgumentNullException(nameof(appDbContext));
    }
   

   
    public Task<Cart> GetByAccountId(Guid accountId, CancellationToken cancellationToken = default)
    {
        
        return Entities.SingleAsync(it => it.AccountId == accountId, cancellationToken);
    }
}