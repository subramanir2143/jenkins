using System;
using System.Net;
using Cnx.Notification.API.Domain.Events;
using Cnx.Notification.API.Domain.Services;
using Cnx.Notification.API.Infrastructure;
using Cnx.Notification.API.Infrastructure.Filters;
using Cnx.Notification.API.Infrastructure.SignalRHub;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cnx.Notification.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));

            services.AddCustomMvc(Configuration).
                AddEventBus(Configuration).
                AddApplicationInsights(Configuration).
                AddSmtpClient().
                AddCustomSwagger(Configuration).
                AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var pathBase = Configuration["PATH_BASE"];
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<MessageHub>("/messagehub");
            });
            app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Notification.API V1");
                c.OAuthClientId("Notification");
                c.OAuthAppName("Notification");
            });
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
          
        }
    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddSmtpClient(this IServiceCollection services)
        {
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<ISmtpClient>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                return new SmtpClientWrapper()
                {
                    Host = config.GetValue<string>("SmtpHost"),
                    Port = config.GetValue<int>("SmtpPort"),
                    Credentials = new NetworkCredential(
                            config.GetValue<String>("SmtpUsername"),
                            config.GetValue<String>("SmtpPassword")
                        )
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            var orchestratorType = configuration.GetValue<string>("OrchestratorType");

            if (orchestratorType?.ToUpper() == "K8S")
            {
                // Enable K8s telemetry initializer
                services.AddApplicationInsightsKubernetesEnricher();
            }

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();  //Injecting Controllers themselves thru DI
                                              //For further info see: http://docs.autofac.org/en/latest/integration/aspnetcore.html#controllers-as-services

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .WithOrigins(new string[] { configuration.GetValue<string>("CorsOrigin") })
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INotificationEventSubscriberService, NotificationEventHandler>();
            services.AddCap(options =>
            {
                options.UseMongoDB(confmon =>
                {
                    confmon.DatabaseConnection = configuration["MongoConnectionString"];
                    confmon.DatabaseName = "capNotification";
                });

                options.UseRabbitMQ(conf =>
                {
                    conf.HostName = configuration["EventBusConnection"];
                    if (!string.IsNullOrWhiteSpace(configuration["EventBusPort"]))
                    {
                        conf.Port = Convert.ToInt32(configuration["EventBusPort"]);
                    }
                    if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                    {
                        conf.UserName = configuration["EventBusUserName"];
                    }
                    if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                    {
                        conf.Password = configuration["EventBusPassword"];
                    }
                });
                options.UseDashboard();

                if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                {
                    options.FailedRetryCount = int.Parse(configuration["EventBusRetryCount"]);
                }

                if (!string.IsNullOrEmpty(configuration["SubscriptionClientName"]))
                {
                    options.DefaultGroup = configuration["SubscriptionClientName"];
                }
            });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Notification HTTP API",
                    Version = "v1",
                    Description = "The Notification HTTP API",
                    TermsOfService = "Terms Of Service"
                });

            });

            return services;
        }

    }
}
