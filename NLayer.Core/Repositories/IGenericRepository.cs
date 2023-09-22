using System.Linq.Expressions;

namespace NLayer.Core.Repositories
{
    public interface GenericRepository<T> where T : class
    {

        IQueryable<T> GetAll();
        Task<T> GetByIdAsycn(int id);
        Task<T> GetByIdsAsycn(int id, int id2);
        IQueryable<T> Where(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsycn(Expression<Func<T, bool>> filter);
        Task AddAsycn(T entity);
        Task AddRangeAsycn(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entites);



    }
}
