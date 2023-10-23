using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface ILocationservicecs : IService<Location>
    {
        Task<List<Location>> GetUserLocations(int productId);
    }
}
