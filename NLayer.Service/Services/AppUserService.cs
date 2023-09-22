using AutoMapper;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class AppUserService : Service<AppUser>, IAppUserService
    {
        private readonly IAppUserRepository _AppUserRepository;
        private readonly IMapper _mapper;

        public AppUserService(GenericRepository<AppUser> repository, IUnitOfWork unitOfWork, IAppUserRepository appUserRepository, IMapper mapper) : base(repository, unitOfWork)
        {
            _AppUserRepository=appUserRepository;
            _mapper=mapper;
        }

        public async Task<AppUser> GetByIdWithAsync(int id)
        {
            return await _AppUserRepository.GetByIdWithAsync(id);
        }
    }
}
