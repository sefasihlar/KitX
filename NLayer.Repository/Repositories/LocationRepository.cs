using Microsoft.EntityFrameworkCore;
using NLayer.Core.Concreate;
using NLayer.Core.Repositories;
using NLayer.Repository.Concreate;

namespace NLayer.Repository.Repositories
{
    public class LocationRepository : GenericRepositoy<Location>, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<List<Location>> GetUserLocations(int productId)
        {
            return await _context.Locations
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }
    }
}
