using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class PersonelProductFeatureRepository : GenericRepositoy<PersonProductFeature>, IPersonelProductFeatureRepository
    {
        public PersonelProductFeatureRepository(AppDbContext context) : base(context)
        {
        }

        public Task<PersonProductFeature> FindByProductIdAsync(int productId)
        {
            return _context.PersonProductFeature
               .Include(x => x.Product)
                .ThenInclude(x => x.Category)
               .FirstOrDefaultAsync(x => x.ProductId == productId);
        }
    }
}
