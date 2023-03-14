using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.RepositoriesInterfaces;

public interface ICartRepository : IRepository<Cart>
{
    
    Task<Cart> GetByAccountId(Guid accountId, CancellationToken cancellationToken=default );

//     Task<Cart> GetById(Guid id, CancellationToken cancellationToken);
//     Task<IReadOnlyList<Cart>> GetAll(CancellationToken cancellationToken);
//     Task Add(Cart entity, CancellationToken cancellationToken);
//     Task Update(Cart entity, CancellationToken cancellationToken);
//     Task Delete(Guid id, CancellationToken cancellationToken);
}