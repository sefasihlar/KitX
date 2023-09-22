namespace NLayer.Core.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQrCode(string text);
    }
}
