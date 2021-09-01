using System;
using System.Collections;
using Discord.Commands;
using Hangfire;
using Hangfire.PostgreSql;
using Izumi.Data;
using Izumi.Framework.Hangfire;
using Izumi.Services.Discord.Client;
using Izumi.Services.Discord.Client.Impl;
using Izumi.Services.Discord.Client.Options;
using Izumi.Services.Hangfire.BackgroundJobs.UploadEmotes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
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

            services.AddControllers();
            services.AddSwaggerDocument();

            services.AddSingleton(_ =>
                TimeZoneInfo.FindSystemTimeZoneById(_config.GetValue<string>("CronTimezoneId")));

            services.AddSingleton<CommandService>();
            services.AddSingleton<IDiscordClientService, DiscordClientService>();
            services.AddScoped<IUploadEmotesJob, UploadEmotesJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MigrateDb(app.ApplicationServices);

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new AllowAllAuthorizationFilter() }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                       ForwardedHeaders.XForwardedProto
                });

                app.Use(async (context, next) =>
                {
                    if (_config.GetValue<string>("AccessType")?.ToLower() != "whitelist")
                    {
                        await next();
                        return;
                    }

                    var ips = (_config.GetValue<string>("AllowedIps") ?? "").Split(';');
                    var remoteIp = context.Request.Headers["X-Real-IP"].ToString();

                    if (!((IList) ips).Contains(remoteIp))
                    {
                        await context.Response.WriteAsync(@"
                            <!doctype html>
                            <html>
                                <body>
                                    <div>
                                        <h2>Oops, seems that you are not authorized to access this page</h2>
                                    <div>
                                </body>
                            </html>");
                        return;
                    }

                    await next();
                });
            }

            app.UseOpenApi();
            app.UseRouting();
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
    }
}
