using BikeScanner.DI;
using BikeScanner.Server.Controllers;
using BikeScanner.Server.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<TelegramUIController>();
    builder.Services.AddHostedService<TelegramPollHostedService>();
}

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BikeScanner Api", Version = "v1" });
});
builder.Services.AddControllers();
builder.Services.AddBikeScanner(configuration);
builder.Services.AddVKAdSource(configuration);
builder.Services.AddPostgreDataAccess(configuration);
builder.Services.AddTelegramUI(configuration);
builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BikeScanner Api V1");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

