using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class SpecialProductFeatureRepository : GenericRepositoy<SpecialProductFeature>, ISpecialProductFeatureRepository
    {
        public SpecialProductFeatureRepository(AppDbContext context) : base(context)
        {
        }

        public Task<SpecialProductFeature> FindByProductIdAsync(int productId)
        {
            return _context.SpecialProductFeature
                .Include(x => x.Product)
                 .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }
    }
}
