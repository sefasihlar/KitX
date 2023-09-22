using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;

namespace NLayer.Service.Services
{
    public class AnimalService : Service<Animal>, IAnimalService
    {
        public AnimalService(GenericRepository<Animal> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
