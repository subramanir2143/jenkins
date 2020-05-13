using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace Cnx.Payroll.ApiGateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            var identityUrl = _configuration.GetValue<string>("IdentityUrl");
            const string AuthenticationProviderKey = "Bearer";
            services.AddMvcCore()
               .AddAuthorization()
               .AddJsonFormatters();

            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddUrlGroup(new Uri(_configuration["PayrollAPIUrlHC"]), name: "payrollapi-check", tags: new string[] { "payrollapi" })
                .AddUrlGroup(new Uri(_configuration["ReadAPIUrlHC"]), name: "readapi-check", tags: new string[] { "readapi" });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
            });

            services.AddAuthentication(AuthenticationProviderKey)
                .AddJwtBearer(AuthenticationProviderKey, options =>
                {
                    options.Authority = identityUrl;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnAuthenticationFailed = async ctx =>
                        {
                            var message = ctx.Exception.Message;
                        },
                        OnTokenValidated = async ctx =>
                        {

                        },

                        OnMessageReceived = async ctx =>
                        {
                            var resp = ctx.Response.Body.ToString();
                        }
                    };
                });

            services.AddOcelot(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var pathBase = _configuration["PATH_BASE"];

            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseMvc();
            app.UseOcelot().Wait();
        }
    }
}
