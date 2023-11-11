using DealerManagement.Base;
using DealerManagement.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DealerManagement.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseModel
    {
        private readonly DealerManagementDBContext dbContext;

        public GenericRepository(DealerManagementDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<TEntity>> GetAll(CancellationToken cancellationToken, params string[] includes)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();
            if (includes.Any())
            {
                query = includes.Aggregate(query, (current, incl) => current.Include(incl));
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetById(CancellationToken cancellationToken, int id,  string[] includes)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();
            if (includes.Any())
            {
                query = includes.Aggregate(query, (current, incl) => current.Include(incl));
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id,cancellationToken);
        }
        
        public async Task<int> InsertAndGetId(CancellationToken cancellationToken, TEntity entity)
        {
            var entity1 = await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity1.Entity.Id;
        }
        
        public async Task<TEntity> InsertAndGetEntity(CancellationToken cancellationToken, TEntity entity, params string[] includes)
        {
            var entity1 = await dbContext.Set<TEntity>().AddAsync(entity);
            await dbContext.SaveChangesAsync();
            var query = dbContext.Set<TEntity>().AsQueryable();
            if (includes.Any())
            {
                query = includes.Aggregate(query, (current, incl) => current.Include(incl));
            }
            return await query.FirstOrDefaultAsync(x => x.Id == entity1.Entity.Id,cancellationToken);
        }

        public void Insert(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();
        }

        public void InsertRange(List<TEntity> entities)
        {
            dbContext.Set<TEntity>().AddRange(entities);
            dbContext.SaveChanges();
        }
        
        public void Remove(int id)
        {
            var entity = dbContext.Set<TEntity>().Find(id);
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            dbContext.SaveChanges();
        }

        public void RemoveRange(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                dbContext.Set<TEntity>().Remove(entity);
            }
            dbContext.SaveChanges();
        }
    }
}