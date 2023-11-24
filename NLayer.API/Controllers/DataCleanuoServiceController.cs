using Microsoft.Extensions.Hosting;
using NLayer.Core.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

public class DataCleanupService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly ILogService _logService; // Örnek olarak kullanılan bir servis

    public DataCleanupService(ILogService logService)
    {
        _logService = logService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24)); // 1 dakikada bir işlem yap

        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        try
        {
            var values = await _logService.GetAllAsycn();
            var valuesFilter = values.Where(x => x.UserName == null && x.Exception == null);// Silecek verileri getir
            if (valuesFilter != null && valuesFilter.Any())
            {
                await _logService.RemoveRangeAsycn(valuesFilter); // Verileri sil
            }
        }
        catch (Exception ex)
        {
            // Hata durumunda uygun bir işlem yapabilirsiniz
            Console.WriteLine("Hata oluştu: " + ex.Message);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
