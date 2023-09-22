using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.GenericManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class AnimalPhotoService : Service<AnimalPhoto>, IAnimalPhotoService
    {
        public AnimalPhotoService(GenericRepository<AnimalPhoto> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
