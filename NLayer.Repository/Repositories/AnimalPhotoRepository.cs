using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
    public class AnimalPhotoRepository : GenericRepositoy<AnimalPhoto>, IAnimalPhotoRepository
    {
        public AnimalPhotoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
