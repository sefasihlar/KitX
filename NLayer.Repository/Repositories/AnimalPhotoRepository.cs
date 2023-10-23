using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class AnimalPhotoRepository : GenericRepositoy<ProductPhoto>, IProductPhotoRepository
    {
        public AnimalPhotoRepository(AppDbContext context) : base(context)
        {
        }
    }
}
