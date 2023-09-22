using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class UserProductService : Service<UserProduct>, IUserProductService
    {
        public UserProductService(GenericRepository<UserProduct> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
