using System.Net.Mail;
using EasyNetQ;
using Events.Contracts;

namespace Notification.Worker.Services;

public class SubscriptionStarterService(IBus bus) : BackgroundService
{
    private const string SmtpHost = "maildev";
    private const int SmtpPort = 1025;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        bus.PubSub.SubscribeAsync<UserCreatedMessage>(
            "welcome-email-service-id",
            async msg => await HandleUserCreatedMessage(msg, stoppingToken),
            cancellationToken: stoppingToken);

        return Task.CompletedTask;
    }

    private async Task HandleUserCreatedMessage(UserCreatedMessage message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"[NOTIFICATION WORKER] Processing welcome email for: {message.Email}");

        // --- Lógica de Envío de Correo (Simulada) ---
        var client = new SmtpClient(SmtpHost, SmtpPort)
        {
            EnableSsl = false
        };
        var mail = new MailMessage(
            "noreply@authservice.com",
            message.Email,
            "Welcome to the Platform!",
            $"Dear {message.FullName}, your account has been successfully created. We are glad to have you!"
        );

        await client.SendMailAsync(mail, cancellationToken);

        Console.WriteLine($"[NOTIFICATION WORKER] Email successfully sent to MailDev for: {message.Email}");
    }
}
