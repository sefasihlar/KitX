using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class BelongingProductFeatureRepository : GenericRepositoy<BelongingProductFeature>, IBelongingProductfeatureRepository
    {
        public BelongingProductFeatureRepository(AppDbContext context) : base(context)
        {
        }

        public Task<BelongingProductFeature> FindByProductIdAsync(int productId)
        {
            return _context.BelongingProductFeature
                .Include(x => x.Product)
                 .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }
    }
}
