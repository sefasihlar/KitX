using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface IAppUserService : IService<AppUser>
    {
        Task<AppUser> GetByIdWithAsync(int id);
    }
}
