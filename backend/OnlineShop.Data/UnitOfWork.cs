using OnlineShop.Data.Repositories;
using OnlineShop.Domain.RepositoriesInterfaces;

namespace OnlineShop.Data;

public class UnitOfWork: IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private IAccountRepository? _accountRepository;
    private IProductRepository? _productRepository;
    private ICartRepository? _cartRepository;


    public IAccountRepository AccountRepository
    {
        get { return _accountRepository ?? new AccountRepository(_dbContext); }
    }
    public IProductRepository ProductRepository
    {
        get { return _productRepository ?? new ProductRepository(_dbContext); }
    }
    public ICartRepository CartRepository
    {
        get { return _cartRepository ?? new CartRepository(_dbContext); }
    }

    public async ValueTask CommitAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }



    public void Dispose() => _dbContext.Dispose();
    public async ValueTask DisposeAsync() => _dbContext.DisposeAsync();

}

