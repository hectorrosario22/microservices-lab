using Auth.Api.Data;
using Auth.Api.Endpoints;
using Auth.Api.Repositories;
using Auth.Api.Security;
using Auth.Api.Services;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQ.Serialization.SystemTextJson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IMongoDatabaseConnection, MongoDatabaseConnection>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var rabbitMQConnectionString = builder.Configuration.GetConnectionString("RabbitMQ")
    ?? throw new InvalidOperationException("RabbitMQ connection string is not configured.");
builder.Services.AddSingleton(RabbitHutch.CreateBus(rabbitMQConnectionString,
    x => x.Register<ISerializer, SystemTextJsonSerializer>(Lifetime.Singleton)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapUserEndpoints();

app.Run();
