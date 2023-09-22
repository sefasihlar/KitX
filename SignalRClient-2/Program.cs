using Microsoft.AspNetCore.SignalR.Client;

HubConnection connection = new HubConnectionBuilder()
    .WithUrl("https://kursdefteri.com.tr/ip-hub")
    .Build();
await connection.StartAsync();

Console.WriteLine($"ConnectionId:{connection.ConnectionId}");

connection.On<string>("QrCodeRead", message =>
{
    Console.WriteLine($"Message: {message}");
});

while (true)
{
    if (Console.ReadKey().Key == ConsoleKey.M)
    {
        Console.Write("Mesaj: ");
        string message = Console.ReadLine();
        Console.WriteLine();
        await connection.InvokeAsync("SendQrCodeReadMessageAsync", message);
    }
}


