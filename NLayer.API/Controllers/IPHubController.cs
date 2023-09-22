using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLayer.API;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class IPHubController : ControllerBase
{
    private readonly IHubContext<IPHub> _hubContext;

    public IPHubController(IHubContext<IPHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestQrCodeRead()
    {
        try
        {
            // QR kod okutulduğunda sinyal gönder
            await _hubContext.Clients.All.SendAsync("QrCodeRead", "Qr code okutuldu.");
            return Ok("Qr code okutuldu sinyali gönderildi.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Hata: {ex.Message}");
        }
    }
}