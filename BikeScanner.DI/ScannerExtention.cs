﻿using BikeScanner.Application.Configs;
using BikeScanner.Application.Jobs;
using BikeScanner.Application.Services.NotificationFactory;
using BikeScanner.Application.Services.SearchService;
using BikeScanner.Application.Services.SubscriptionsService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeScanner.DI
{
    public static class ScannerExtention
    {
        public static void AddBikeScanner(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JobsConfig>(configuration.GetSection(nameof(JobsConfig)));

            services.AddSingleton<INotificatorFactory, NotificatorFactory>();

            services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            services.AddScoped<ISearchService, SearchService>();

            services.AddScoped<IContentIndexatorJob, ContentIndexatorJob>();
            services.AddScoped<IAutoSearchJob, AutoSearchJob>();
            services.AddScoped<INotificationsSenderJob, NotificationsSenderJob>();
            services.AddScoped<ScanJobsChain>();
        }
    }
}
