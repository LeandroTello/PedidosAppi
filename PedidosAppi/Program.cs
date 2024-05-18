using PedidosAppi.Data;
using PedidosAppi.Interfaces;
using PedidosAppi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PedidosAppiContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PedidosAppiContext") 
    ?? throw new InvalidOperationException("Connection string 'PedidosAppiContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ISendEmailService, SendEmailService>();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

builder.Configuration.AddConfiguration(config);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", app =>
    {
        app.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//builder.WebHost.UseUrls("https://localhost:7273");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();