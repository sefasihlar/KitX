using Microsoft.AspNetCore.SignalR;
namespace NLayer.API
{
    public sealed class IPHubb : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("Message", $"{Context.ConnectionId} id'li bağlantı acildi");
        }

        // Bu metot sadece hub üzerinden sinyal göndermek için kullanılır.
        public async Task SendQrCodeReadMessageAsync(string message)
        {
            await Clients.All.SendAsync("QrCodeRead", message);
        }
    }
}
