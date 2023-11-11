using DealerManagement.Base;

namespace DealerManagement.Data.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseModel
    {
        Task<List<TEntity>> GetAll(CancellationToken cancellationToken, params string[] includes);
        Task<TEntity> GetById(CancellationToken cancellationToken, int id, params string[] includes);
        void Remove(int id);
        void Remove(TEntity entity);
        void Insert(TEntity entity);
        Task<int> InsertAndGetId(CancellationToken cancellationToken, TEntity entity);
        Task<TEntity> InsertAndGetEntity(CancellationToken cancellationToken, TEntity entity, params string[] includes);
        void InsertRange(List<TEntity> entities);
        void RemoveRange(List<TEntity> entities);
    }
}