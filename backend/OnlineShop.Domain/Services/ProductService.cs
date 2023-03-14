using OnlineShop.Data;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;

namespace OnlineShop.Domain.Services;

public class ProductService
{
    
    private IUnitOfWork _uow;
    public ProductService( IUnitOfWork uow)
    {
        if (uow == null) throw new ArgumentNullException(nameof(uow));
        _uow = uow;
    }
    
    public async Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
    {
        var products = await _uow.ProductRepository.GetAll(cancellationToken);
        return products;
    }
    public async Task<Product> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var product = await _uow.ProductRepository.GetById(id, cancellationToken);
        return product;
    }
    public async Task AddProduct(Product product, CancellationToken cancellationToken)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        
        await _uow.ProductRepository.Add(product, cancellationToken);
    }
    public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
    {
        if (product == null) throw new ArgumentNullException(nameof(product));
        await _uow.ProductRepository.Update(product, cancellationToken);
    }
    public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
    {
        await _uow.ProductRepository.Delete(id, cancellationToken);
    }
}