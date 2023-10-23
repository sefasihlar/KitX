namespace NLayer.Core.Services
{
    public interface IIHubService
    {
        Task SendQrCodeReadMessageAsync(string IPAddress);
    }
}
