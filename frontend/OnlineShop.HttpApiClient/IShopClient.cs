using OnlineShop.Domain.Entities;
using OnlineShop.Models.Requests;
using OnlineShop.Models.Responses;

namespace OnlineShop.HttpClient
{
    public interface IShopClient
    {
        Task<IReadOnlyList<Product>> GetProducts(CancellationToken cancellationToken);
        Task<RegisterResponse> Register(RegisterRequest request, CancellationToken cancellationToken);
        Task<LogInResponse> Login(LogInRequest request, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task DeleteProduct(Guid id, CancellationToken cancellationToken);
        Task AddProduct(Product product, CancellationToken cancellationToken);
        public void SetAuthToken(string token);
    }
}