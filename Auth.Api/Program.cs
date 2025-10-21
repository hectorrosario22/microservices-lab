using Auth.Api.Data;
using Auth.Api.Entities;
using Auth.Api.Requests;
using Auth.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<MongoDatabaseConnection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapPost("/users", (
    UserRegistrationRequest request,
    MongoDatabaseConnection dbConnection) =>
{
    // TODO: Validate request
    var usersCollection = dbConnection.Database?.GetCollection<User>("users");
    var newUser = new User
    {
        Username = request.Username,
        PasswordHash = PasswordHasher.HashPassword(request.Password),
        FirstName = request.FirstName,
        LastName = request.LastName,
        Email = request.Email,
    };
    usersCollection?.InsertOne(newUser);
    return Results.NoContent();
})
.WithName("RegisterUser");

app.Run();
