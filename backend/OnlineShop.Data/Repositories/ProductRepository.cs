using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;

namespace OnlineShop.Data.Repositories
{
    public class ProductRepository : EfRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            if (appDbContext == null) throw new ArgumentNullException(nameof(appDbContext));
        }

        // public  async Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken)
        //
        // {
        //     return await _appDbContext.Products.ToListAsync(cancellationToken);
        //
        // }
        //
        // public async Task<Product> GetById(Guid id,CancellationToken cancellationToken)
        // {
        //     
        //     return await _appDbContext.Products.FirstAsync(p => p.Id == id,cancellationToken);
        // }
        //
        //
        // public async Task Add(Product product,CancellationToken cancellationToken)
        // {
        //     
        //     _appDbContext.Products.Add(product);
        //     await _appDbContext.SaveChangesAsync(cancellationToken);
        // }
        //
        //
        //
        //
        // public async Task Update(Product product,CancellationToken cancellationToken)
        // {
        //     
        //   _appDbContext.Entry(product).State = EntityState.Modified;
        //      await _appDbContext.SaveChangesAsync(cancellationToken);
        // }
        //
        // public async Task Delete(Guid id,CancellationToken cancellationToken)
        // {
        //     var product = await _appDbContext.Products.FirstAsync(p => p.Id == id, cancellationToken);
        //     _appDbContext.Products.Remove(product);
        //     await _appDbContext.SaveChangesAsync(cancellationToken);
        //     
        // }
    }
}