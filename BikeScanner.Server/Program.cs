using BikeScanner.Application.Jobs;
using BikeScanner.DI;
using BikeScanner.Server;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.OpenApi.Models;
using NLog.Web;
using TelegramBot.UI.DI;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddBikeScannerBot(configuration);
builder.Services.AddTelegramPolling();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BikeScanner Api", Version = "v1" });
//});
builder.Services.AddBikeScanner(configuration);
//builder.Services.AddVKAdSource(configuration);
builder.Services.AddPostgreDataAccess(configuration);
builder.Services.AddServices();
//builder.Services.AddHangfire(options =>
//    options.UsePostgreSqlStorage(configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddHangfireServer();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (builder.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BikeScanner Api V1");
//    });
//}
//GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.Services));
//GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
//app.UseHangfireDashboard();

//// Initializations
//RecurringJob.AddOrUpdate<ScanJobsChain>("SCAN_CHAIN", j => j.ExecuteChain(), Cron.Hourly);

app.Run();


