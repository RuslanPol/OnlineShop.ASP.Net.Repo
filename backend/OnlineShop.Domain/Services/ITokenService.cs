using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(Account account);
    }
}