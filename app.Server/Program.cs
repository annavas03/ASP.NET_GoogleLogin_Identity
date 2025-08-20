using app.Server.Data;
using app.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity Framework Core services

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllers();

// Configure JSON serialization options
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
try
{
    Console.WriteLine("Starting app...");
    var app = builder.Build();

    app.UseCors("AllowReact");
    app.MapControllers();

    Console.WriteLine("Running app...");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}