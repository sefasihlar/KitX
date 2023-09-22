using Microsoft.EntityFrameworkCore;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using System.Linq.Expressions;

namespace NLayer.Service.GenericManager
{
    public class Service<T> : IService<T> where T : class
    {
        private readonly GenericRepository<T> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(GenericRepository<T> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;

        }

        public async Task<T> AddAsycn(T entity)
        {
            await _repository.AddAsycn(entity);
            await _unitOfWork.CommitAsycn();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsycn(IEnumerable<T> entities)
        {
            await _repository.AddRangeAsycn(entities);
            await _unitOfWork.CommitAsycn();
            return entities;
        }

        public async Task<bool> AnyAsycn(Expression<Func<T, bool>> filter)
        {
            return await _repository.AnyAsycn(filter);
        }

        public async Task<IEnumerable<T>> GetAllAsycn()
        {
            return await _repository.GetAll().ToListAsync();
        }

        public async Task<T> GetByIdAsycn(int id)
        {
            return await _repository.GetByIdAsycn(id);
        }

        public async Task<T> GetByIdsAsycn(int id, int id2)
        {
            return await _repository.GetByIdsAsycn(id, id2);
        }

        public async Task RemoveAsycn(T entity)
        {
            _repository.Remove(entity);
            await _unitOfWork.CommitAsycn();
        }

        public async Task RemoveRangeAsycn(IEnumerable<T> entites)
        {
            _repository.RemoveRange(entites);
            await _unitOfWork.CommitAsycn();
        }

        public async Task UpdateAsycn(T entity)
        {
            _repository.Update(entity);
            await _unitOfWork.CommitAsycn();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> filter)
        {
            return _repository.Where(filter);
        }
    }

}
