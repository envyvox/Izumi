using System;
using Discord.Commands;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Izumi.Data;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Impl;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Game.Localization;
using Izumi.Services.Game.Localization.Impl;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteContract;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreCastle;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteExploreGarden;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteFieldWatering;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteFishing;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteMaking.Impl;
using Izumi.Services.Hangfire.BackgroundJobs.CompleteUserTransit;
using Izumi.Services.Hangfire.BackgroundJobs.EnergyRecovery;
using Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopBanner;
using Izumi.Services.Hangfire.BackgroundJobs.GenerateDynamicShopRecipe;
using Izumi.Services.Hangfire.BackgroundJobs.StartNewDay;
using Izumi.Services.Hangfire.BackgroundJobs.Unmute;
using Izumi.Services.Hangfire.BackgroundJobs.VoiceStatistic;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;

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
            });

            services.AddHangfireServer();
            services.AddHangfire(config =>
            {
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute {Attempts = 0});
                config.UsePostgreSqlStorage(_config.GetConnectionString("main"));
            });

            services.AddAutoMapper(typeof(IDiscordClientService).Assembly);
            services.AddMediatR(typeof(IDiscordClientService).Assembly);

            services
                .AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            services.AddOpenApiDocument();

            services.AddSingleton(_ =>
                TimeZoneInfo.FindSystemTimeZoneById(_config.GetValue<string>("CronTimezoneId")));

            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
            services.AddSingleton<ILocalizationService, LocalizationService>();

            services.AddScoped<ICompleteUserTransitJob, CompleteUserTransitJob>();
            services.AddScoped<ICompleteExploreGardenJob, CompleteExploreGardenJob>();
            services.AddScoped<ICompleteExploreCastleJob, CompleteExploreCastleJob>();
            services.AddScoped<ICompleteFishingJob, CompleteFishingJob>();
            services.AddScoped<ICompleteCookingJob, CompleteCookingJob>();
            services.AddScoped<ICompleteCraftingItemJob, CompleteCraftingItemJob>();
            services.AddScoped<ICompleteCraftingAlcoholJob, CompleteCraftingAlcoholJob>();
            services.AddScoped<ICompleteCraftingDrinkJob, CompleteCraftingDrinkJob>();
            services.AddScoped<ICompleteFieldWateringJob, CompleteFieldWateringJob>();
            services.AddScoped<IStartNewDayJob, StartNewDayJob>();
            services.AddScoped<IEnergyRecoveryJob, EnergyRecoveryJob>();
            services.AddScoped<IGenerateDynamicShopBannerJob, GenerateDynamicShopBannerJob>();
            services.AddScoped<ICompleteContractJob, CompleteContractJob>();
            services.AddScoped<IUnmuteJob, UnmuteJob>();
            services.AddScoped<IVoiceStatisticJob, VoiceStatisticJob>();
            services.AddScoped<IGenerateDynamicShopRecipeJob, GenerateDynamicShopRecipeJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MigrateDb(app.ApplicationServices);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] {new AllowAllAuthorizationFilter()}
            });

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .Build());

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.StartDiscord();
        }

        private static void MigrateDb(IServiceProvider serviceProvider)
        {
            using var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
            context.Database.Migrate();
        }

        private class AllowAllAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }
    }
}