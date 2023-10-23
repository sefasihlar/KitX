using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface ILocationRepository : GenericRepository<Location>
    {
        Task<List<Location>> GetUserLocations(int productId);
    }
}
