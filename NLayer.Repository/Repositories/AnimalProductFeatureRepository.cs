using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class AnimalProductFeatureRepository : GenericRepositoy<AnimalProductFeature>, IAnimalProductFeatureRepository
    {
        public AnimalProductFeatureRepository(AppDbContext context) : base(context)
        {
        }

        public Task<AnimalProductFeature> FindByProductIdAsync(int productId)
        {
            return _context.AnimalProductFeature
                 .Include(x => x.Product)
                 .ThenInclude(x => x.Category)
                 .FirstOrDefaultAsync(x => x.ProductId == productId);

        }
    }
}
