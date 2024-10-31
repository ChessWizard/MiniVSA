using Carter;
using FluentValidation;
using HealthChecks.UI.Client;
using Mapster;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using MiniVSA.CatalogService.Application.Behaviours;
using MiniVSA.CatalogService.Application.Mappings;
using MiniVSA.CatalogService.Infrastructure.Data;
using MiniVSA.CatalogService.Infrastructure.Filters.Exception;
using Weasel.Core;

namespace MiniVSA.CatalogService
{
    public static class ServiceRegistrations
    {
        #region Services

        public static void AddLibraries(this IServiceCollection services)
        {
            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniVSA.Catalog Service API", Version = "v1" });
            });

            // Carter
            services.AddCarter();

            // MediatR
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
                configuration.AddOpenBehavior(typeof(ValidationBehaviour<,>));
                configuration.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        }

        public static void AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                    .AddNpgSql(configuration.GetConnectionString("DbConnection")!);
        }

        public static void AddProjectSettings(this IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = ".MiniVSA.Antiforgery";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            services.AddExceptionHandler<GlobalExceptionHandler>();
        }

        public static void AddBackingServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddMarten(options =>
            {
                options.Connection(configuration.GetConnectionString("DbConnection")!);
                options.AutoCreateSchemaObjects = AutoCreate.None;
                new MartenConfiguration().Configure(null, options);
            }).UseLightweightSessions();
        }

        public static void AddMappingProfiles(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            new BrandMappingProfile().Register(config);
        }

        #endregion

        #region App

        public static void ConfigureLibraries(this WebApplication app)
        {
            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DisplayRequestDuration();
                });
            }

            // Carter
            app.MapCarter();
        }

        public static void ConfigureHealthCheck(this WebApplication app)
        {
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
        }

        public static void ConfigureProjectSettings(this WebApplication app)
        {
            app.UseAntiforgery();
            app.UseExceptionHandler(options => { });
        }

        #endregion
    }
}
