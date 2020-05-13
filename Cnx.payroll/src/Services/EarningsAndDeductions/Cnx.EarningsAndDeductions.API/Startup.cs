using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cnx.EarningsAndDeductions.API.Infrastructure.Filters;
using Cnx.EarningsAndDeductions.API.Services;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Domain.Interfaces;
using Cnx.EarningsAndDeductions.Infrastructure;
using Cnx.EarningsAndDeductions.Infrastructure.Cache;
using Cnx.EarningsAndDeductions.Infrastructure.EventStore;
using Cnx.EarningsAndDeductions.Infrastructure.Repositories;
using Cnx.Payroll.API.Infrastructure.AutofacModules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Cnx.EarningsAndDeductions.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
            .AddCustomSwagger(Configuration)
            .AddApplicationInsights(Configuration)
            .AddHealthChecks(Configuration)
            .AddCustomDbContext(Configuration);

            #region CQRSlite    

            services.AddMemoryCache();
            services.AddScoped<IDbSession, Session>();
            services.AddSingleton<IEventStore, MongoEventStore>();
            services.AddScoped<ICache, MemoryCache>();
            services.AddScoped<IRepository>(y => new CacheRepository(new Repository(y.GetService<IEventStore>()), y.GetService<IEventStore>(), y.GetService<ICache>()));
            #endregion

            services.Configure<EDSettings>(Configuration);
            services.Configure<FileSettings>(Configuration);

            var container = new ContainerBuilder();
            container.Populate(services);
            container.RegisterModule(new MediatorModule());
            container.RegisterModule(new ApplicationModule());
            return new AutofacServiceProvider(container.Build());
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

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "EarningsAndDeductions.API V1");
                  c.OAuthClientId("EarningAndDeductions");
                  c.OAuthAppName("Earnings and Deductions UI");
              });
        }
    }

    static class CustomExtensionsMethods
    {

        internal static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EDModelContext>(options =>
            {
                options.UseMySql(configuration["MySQLConnectionString"],
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                    });
            },
                       ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                   );

            return services;
        }
        internal static IServiceCollection AddCustomMvc(this IServiceCollection services)
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
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }
        internal static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "Earnings and Deductions HTTP API",
                    Version = "v1",
                    Description = "The Earning and Deductions HTTP API",
                    TermsOfService = "Terms Of Service"
                });
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

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            var hcBuilder = services.AddHealthChecks();

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

            hcBuilder.AddMySql(
                    configuration["MySQLConnectionString"],
                    name: "ReadDB-check",
                    tags: new string[] { "readdb" });

            // #TODO: Add healthcheck for EventBus

            return services;
        }
    }
}
