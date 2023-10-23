using Microsoft.AspNetCore.SignalR;
using NLayer.Core.Services;

namespace NLayer.Service.Services
{
    public class IPHubService : Hub, IIHubService
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("Message", $"{Context.ConnectionId} id'li bağlantı acildi");
        }

        public async Task SendQrCodeReadMessageAsync(string IPAddress)
        {
            await Clients.All.SendAsync("QrCodeRead", IPAddress);
        }

        // Bu metot sadece hub üzerinden sinyal göndermek için kullanılır.
        //public async Task SendQrCodeReadMessageAsync(string message)
        //{
        //    await Clients.All.SendAsync("QrCodeRead", message);
        //}
    }
}
