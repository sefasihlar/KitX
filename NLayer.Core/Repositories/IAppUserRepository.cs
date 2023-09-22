using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IAppUserRepository : GenericRepository<AppUser>
    {
        Task<AppUser> GetByIdWithAsync(int id);
    }
}
