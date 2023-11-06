using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NLayer.Core.Services;
using NLayer.Service.Services;

[EnableCors("AllowMyOrigin")]
[Authorize(AuthenticationSchemes = "Roles")]
[Route("api/[controller]")]
[ApiController]
public class IPHubController : ControllerBase
{
    private readonly IHubContext<IPHubService> _hubContext;
    private readonly IIHubService _hubService;


    public IPHubController(IHubContext<IPHubService> hubContext, IIHubService hubService)
    {
        _hubContext = hubContext;
        _hubService=hubService;
    }

    [HttpGet("test")]
    public async Task<IActionResult> TestQrCodeRead()
    {
        try
        {
            // QR kod okutulduğunda sinyal gönder

            await _hubContext.Clients.All.SendAsync("QrCodeRead", "Swagger Üzerinden Test Bildirimi gönderildi");
            return Ok("Qr code okutuldu sinyali gönderildi.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Hata: {ex.Message}");
        }
    }
}