using OnlineShop.Domain.RepositoriesInterfaces;

namespace OnlineShop.Data;

public interface IUnitOfWork : IDisposable,IAsyncDisposable
{
    IAccountRepository AccountRepository { get; }
    IProductRepository ProductRepository { get; }
    ICartRepository CartRepository { get; }
    ValueTask CommitAsync(CancellationToken cancellationToken = default);
    
    
}