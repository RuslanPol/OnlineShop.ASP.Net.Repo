namespace OnlineShop.Domain.RepositoriesInterfaces
{
    public interface IRepository<TEntity>
    {
        public Task<TEntity> GetById(Guid id, CancellationToken cancellationToken = default);
        public Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken = default);
        public Task Add(TEntity entity, CancellationToken cancellationToken = default);
        public Task Update(TEntity entity, CancellationToken cancellationToken = default);
        public Task Delete(Guid id, CancellationToken cancellationToken = default);
    }
}