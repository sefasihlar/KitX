using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IIPAddressRepository : GenericRepository<IPAddress>
    {
        Task<List<IPAddress>> GetWithProductListAsync();
        Task<List<IPAddress>> GetByIpAddressWithProductId(int productId);
    }
}
