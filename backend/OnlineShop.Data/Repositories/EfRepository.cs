using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.RepositoriesInterfaces;


namespace OnlineShop.Data.Repositories
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly AppDbContext DbContext;

        public EfRepository(AppDbContext dbContext) =>
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        public DbSet<TEntity> Entities => DbContext.Set<TEntity>();


        public virtual Task<TEntity> GetById(Guid id, CancellationToken cancellationToken)
            => Entities.FirstAsync(it => it.Id == id, cancellationToken);

        public virtual async Task<IReadOnlyList<TEntity>> GetAll(CancellationToken cancellationToken)
            => await Entities.ToListAsync(cancellationToken);

        public virtual async Task Add(TEntity entity, CancellationToken cancellationToken)
        {
            await Entities.AddAsync(entity, cancellationToken);
            // await DbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task Update(TEntity entity, CancellationToken cancellationToken)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            //await DbContext.SaveChangesAsync(cancellationToken);
        }


        public virtual async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var entity = await Entities.FirstAsync(p => p.Id == id, cancellationToken);
            Entities.Remove(entity);
            //await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}