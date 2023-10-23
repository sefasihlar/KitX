using NLayer.Core.Concreate;

namespace NLayer.Core.Repositories
{
    public interface IQRCodeRepository : GenericRepository<QrCode>
    {
        //Task<byte[]> QrCodeToProductAsync(int id);
        //Task<QrCode> GetByUserProduct(int productId);
        //Task<QrCode> GetAnimalWithProductId(int id);
        //Task<List<QrCode>> GetProductWithUserId(int id);
        Task<List<QrCode>> GetUserProduct(int userId);
        Task<QrCode> GetByQrCodeWithProductId(int productId);

    }
}
