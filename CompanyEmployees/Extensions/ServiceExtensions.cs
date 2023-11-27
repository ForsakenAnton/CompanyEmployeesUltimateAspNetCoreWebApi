using Contracts;
using LoggerService;
using Repository;
using Service.Contracts;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using CompanyEmployees.Presentation.Controllers;

namespace CompanyEmployees.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination"));
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options =>
        {

        });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();

    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();

    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

    public static void ConfigureSqlContext(
        this IServiceCollection services,
        IConfiguration configuration) 
    {
        //services.AddSqlServer<RepositoryContext>(
        //    configuration.GetConnectionString("sqlConnection"));

        services
            .AddDbContext<RepositoryContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    }


    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => 
            config.OutputFormatters.Add(new CsvOutputFormatter()));


    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            SystemTextJsonOutputFormatter? systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()
                ?.FirstOrDefault();
            if (systemTextJsonOutputFormatter != null)
            {
                systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+json");

                systemTextJsonOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.apiroot+json");
            }

            XmlDataContractSerializerOutputFormatter? xmlOutputFormatter = config.OutputFormatters
            .OfType<XmlDataContractSerializerOutputFormatter>()
            ?.FirstOrDefault();
            if (xmlOutputFormatter != null)
            {
                xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.hateoas+xml");

                xmlOutputFormatter.SupportedMediaTypes
                    .Add("application/vnd.codemaze.apiroot+xml");
            }
        });
    }


    // use Asp.Versioning.Mvc
    // Microsoft.AspNetCore.Mvc.Versioning - Deprecated
    public static void ConfigureVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(opt =>
        {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;

            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            opt.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.HeaderApiVersionReader("api-version");

            opt.Conventions.Controller<CompaniesController>()
                .HasApiVersion(new ApiVersion(1, 0));
            opt.Conventions.Controller<CompaniesV2Controller>()
                .HasDeprecatedApiVersion(new ApiVersion(2, 0));

            //opt.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.QueryStringApiVersionReader("api-version");

            //opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
            //opt.ApiVersionReader = new Asp.Versioning.HeaderApiVersionReader("api-version");
        });
    }


    public static void ConfigureResponseCaching(this IServiceCollection services)
    {
        services.AddResponseCaching();
    }

    public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
    {
        services.AddHttpCacheHeaders(
            expirationModelOptionsAction: (expirationOpt) =>
            {
                expirationOpt.MaxAge = 65;
                expirationOpt.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            },
            validationModelOptionsAction: (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });
    }

}