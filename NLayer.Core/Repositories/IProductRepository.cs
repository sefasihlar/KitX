using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IProductRepository : GenericRepository<Product>
    {
        Task<byte[]> QrCodeToProductAsync(int id);
        Task<Product> GetByUserProduct(int productId);
        Task<Product> GetAnimalWithProductId(int id);
        Task<List<Product>> GetProductWithUserId(int id);
    }
}
