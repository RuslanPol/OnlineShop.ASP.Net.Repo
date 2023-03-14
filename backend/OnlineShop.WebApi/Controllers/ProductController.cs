using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;
using OnlineShop.Domain.Services;

namespace OnlineShop.WebApi.Controllers
{
    [Route("Products")]
    public class ProductController : ControllerBase
    {
        
        private readonly ProductService _productService;


        public ProductController(IProductRepository productRepository,ProductService productService)
        {
           
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        [HttpGet("get_all")]
        public async Task<IEnumerable<Product>> GetProducts(CancellationToken cancellationToken)
        {
            var products = await _productService.GetProducts(cancellationToken);
            return products;
        }

        [HttpGet("get_by_id/{id:guid}")]
        public async Task<Product> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductById(id, cancellationToken);
            return product;
        }

        [HttpPost("add")]
        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            await _productService.AddProduct(product, cancellationToken);
        }

        [HttpPut("update")]
        public async Task UpdateProduct(Product product, CancellationToken cancellationToken)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            await _productService.UpdateProduct(product, cancellationToken);
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task DeleteProduct(Guid id, CancellationToken cancellationToken)
        {
            await _productService.DeleteProduct(id, cancellationToken);
        }
    }
}