using BikeScanner.Application.Jobs;
using BikeScanner.Application.ServiceCollection;
using BikeScanner.Data.Postgre.ServiceCollection;
using BikeScanner.Infrastructure.ServiceCollection;
using BikeScanner.Server.Hangfire;
using Hangfire;
using Hangfire.PostgreSql;
using NLog.Web;
using TelegramBot.UI.ServiceCollection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();

var config = builder.Configuration;

// Add services to the container.
builder.Services
    .AddMemoryCache()
    .AddPostgresDB(config)
    .AddBikeScannerMapping()
    .AddBikeScannerServices()
    .AddBikeScannerJobs(config)
    .AddTelegramNotificator()
    .AddVkCrawler(config)
    .AddBikeScannerTelegramBotUI(config)
    .AddTelegramPollingHostedService()
    .AddHangfire(o => o.UsePostgreSqlStorage(config.GetConnectionString("DefaultConnection")))
    .AddHangfireServer();
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BikeScanner Api", Version = "v1" });
//});

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
GlobalConfiguration.Configuration.UseActivator(new HangfireActivator(app.Services));
GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
app.UseHangfireDashboard();

// Hangfire jobs
RecurringJob.AddOrUpdate<IAdditionalCrawlingJob>(
    Jobs.ADDITIONAL_CRAWLING,
    j => j.Execute(),
    Cron.Never
    );
RecurringJob.AddOrUpdate<INotificationsSenderJob>(
    Jobs.NOTIFICATIONS,
    j => j.Execute(),
    Cron.Never)
    ;

app.Run();


