using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.SystemTextJson;
using Notification.Worker.Services;
using Notification.Worker.Settings;

var builder = Host.CreateApplicationBuilder(args);

var rabbitMQConnectionString = builder.Configuration.GetConnectionString("RabbitMQ")
    ?? throw new InvalidOperationException("RabbitMQ connection string is not configured.");
builder.Services.AddSingleton(RabbitHutch.CreateBus(rabbitMQConnectionString,
    x => x.Register<ISerializer, SystemTextJsonSerializer>(Lifetime.Singleton)));

builder.Services.AddOptions<SmtpSettings>()
    .BindConfiguration(SmtpSettings.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHostedService<SubscriptionStarterService>();

var host = builder.Build();
host.Run();
