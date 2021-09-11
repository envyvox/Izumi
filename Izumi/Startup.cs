using System;
using Discord.Commands;
using Hangfire;
using Hangfire.PostgreSql;
using Izumi.Data;
using Izumi.Framework.Hangfire;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Impl;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Localization.Impl;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreCastle;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreGarden;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteFishing;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking.Impl;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteUserTransit;
using Izumi.Services.Hangfire.BackgroundJobs.UploadEmotes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Izumi
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DiscordOptions>(x => _config.GetSection("Discord").Bind(x));

            services.AddDbContextPool<AppDbContext>(o =>
            {
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                o.UseNpgsql(_config.GetConnectionString("main"),
                    s => { s.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name); });
                o.EnableSensitiveDataLogging();
            });

            services.AddHangfire(config =>
            {
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
                config.UsePostgreSqlStorage(_config.GetConnectionString("main"));
            });

            services.AddAutoMapper(typeof(IDiscordClientService).Assembly);
            services.AddMediatR(typeof(IDiscordClientService).Assembly);

            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSwaggerGen();

            services.AddSingleton(_ =>
                TimeZoneInfo.FindSystemTimeZoneById(_config.GetValue<string>("CronTimezoneId")));

            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            services.AddScoped<IUploadEmotesJob, UploadEmotesJob>();
            services.AddScoped<ICompleteUserTransitJob, CompleteUserTransitJob>();
            services.AddScoped<ICompleteExploreGardenJob, CompleteExploreGardenJob>();
            services.AddScoped<ICompleteExploreCastleJob, CompleteExploreCastleJob>();
            services.AddScoped<ICompleteFishingJob, CompleteFishingJob>();
            services.AddScoped<ICompleteCookingJob, CompleteCookingJob>();
            services.AddScoped<ICompleteCraftingItemJob, CompleteCraftingItemJob>();
            services.AddScoped<ICompleteCraftingAlcoholJob, CompleteCraftingAlcoholJob>();
            services.AddScoped<ICompleteCraftingDrinkJob, CompleteCraftingDrinkJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MigrateDb(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllAuthorizationFilter() }
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Izumi API V1"); });

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.StartDiscord();
        }

        private static void MigrateDb(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }
    }
}
