using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface IIPAddressService : IService<IPAddress>
    {
        Task<List<IPAddress>> GetWithProductListAsync();
        Task<IPAddress> GetByIpAddressWithProductId(int productId);
    }
}
