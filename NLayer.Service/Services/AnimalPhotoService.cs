using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class AnimalPhotoService : Service<ProductPhoto>, IAnimalPhotoService
    {
        public AnimalPhotoService(GenericRepository<ProductPhoto> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
