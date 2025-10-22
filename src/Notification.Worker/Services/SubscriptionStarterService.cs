using System.Net.Mail;
using EasyNetQ;
using Events.Contracts;
using Microsoft.Extensions.Options;
using Notification.Worker.Settings;

namespace Notification.Worker.Services;

public class SubscriptionStarterService(
    IBus bus, IOptions<SmtpSettings> smtpOptions) : BackgroundService
{
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
        var emailBody = $@"""
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }}
                    .container {{ width: 100%; max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border: 1px solid #ddd; border-radius: 8px; }}
                    .header {{ background-color: #007bff; color: white; padding: 10px 0; text-align: center; border-radius: 5px 5px 0 0; }}
                    .content {{ padding: 20px; line-height: 1.6; color: #333; }}
                    .footer {{ margin-top: 20px; padding: 10px; border-top: 1px solid #eee; font-size: 0.8em; text-align: center; color: #777; }}
                    .button {{ background-color: #28a745; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; margin-top: 15px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>¡Bienvenido a la Plataforma MSL!</h2>
                    </div>
                    <div class='content'>
                        <p>Estimado/a <strong>{message.FullName}</strong>,</p>
                        
                        <p>Queremos darte la bienvenida a nuestra plataforma de servicios resilientes. Tu cuenta ha sido creada exitosamente. Estamos emocionados de que te unas a nuestra comunidad.</p>
                        
                        <p><strong>Detalles de tu cuenta:</strong></p>
                        <ul>
                            <li><strong>Usuario:</strong> {message.Email}</li>
                            <li><strong>ID de Usuario:</strong> {message.UserId}</li>
                        </ul>
                        
                        <p>Ahora puedes comenzar a explorar todos nuestros servicios de e-commerce y logística.</p>
                        
                        <a href='http://localhost:8001/login' class='button'>Iniciar Sesión</a>

                        <p style='margin-top: 30px;'>Saludos cordiales,</p>
                        <p>El equipo de la Plataforma MSL</p>
                    </div>
                    <div class='footer'>
                        <p>Este es un correo automático. Por favor, no responder.</p>
                    </div>
                </div>
            </body>
            </html>
            """;

        var client = new SmtpClient(smtpOptions.Value.Host, smtpOptions.Value.Port)
        {
            EnableSsl = smtpOptions.Value.EnableSsl
        };
        var mail = new MailMessage
        {
            From = new MailAddress(smtpOptions.Value.FromAddress, smtpOptions.Value.FromName),
            To =
            {
                new MailAddress(message.Email, message.FullName)
            },
            Subject = "Welcome to the Platform!",
            Body = emailBody,
            IsBodyHtml = true
        };

        await client.SendMailAsync(mail, cancellationToken);

        Console.WriteLine($"[NOTIFICATION WORKER] Email successfully sent to MailDev for: {message.Email}");
    }
}
