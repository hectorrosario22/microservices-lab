using EasyNetQ;
using Events.Contracts;

namespace Notification.Worker;

public class SubscriptionStarterService(IBus bus) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bus.PubSub.SubscribeAsync<UserCreatedMessage>(
            "welcome-email-service-id",
            async msg =>
            {
                // Aquí iría la lógica para manejar el evento, como enviar un correo electrónico.
                Console.WriteLine($"Usuario registrado: {msg.Username}, Email: {msg.Email}");
                await Task.CompletedTask;
            },
            cancellationToken: stoppingToken);

        return Task.CompletedTask;
    }
}
