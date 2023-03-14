using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.RepositoriesInterfaces
{
    public interface IProductRepository:IRepository<Product>
    {
        // Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken=default);
        // Task<Product> GetById(Guid id,CancellationToken cancellationToken=default);
        // Task Add(Product product,CancellationToken cancellationToken=default);
        // Task Update(Product product,CancellationToken cancellationToken=default);
        // Task Delete(Guid id,CancellationToken cancellationToken=default);
        
    }
}