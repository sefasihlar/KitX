using NLayer.Core.Concreate;

namespace NLayer.Core.Services
{
    public interface IQRCodeService : IService<QrCode>
    {
        Task<List<QrCode>> GetUserProduct(int userId);
        Task<QrCode> GetByQrCodeWithProductId(int productId);
    }
}
