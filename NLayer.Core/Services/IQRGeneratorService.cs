namespace NLayer.Core.Services
{
    public interface IQRGeneratorService
    {
        byte[] GenerateQrCode(string text);
    }
}
