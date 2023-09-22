using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;
using System.Linq.Expressions;

namespace NLayer.Repository.Repositories
{
    public class GenericRepositoy<T> : GenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        private readonly DbSet<T> _dbSet;

        public GenericRepositoy(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsycn(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsycn(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public async Task<bool> AnyAsycn(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AnyAsync(filter);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsQueryable();
            //çekilen veriler memoride tutulmaması için AsNoTracking kullandık
        }

        public async Task<T> GetByIdAsycn(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByIdsAsycn(int id, int id2)
        {
            return await _dbSet.FindAsync(id, id2);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entites)
        {
            _dbSet.RemoveRange(entites);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return _dbSet.Where(filter);
        }
    }
}
