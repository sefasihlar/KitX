using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Commit()
        {
            _appDbContext.SaveChanges();
        }

        public async Task CommitAsycn()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}
