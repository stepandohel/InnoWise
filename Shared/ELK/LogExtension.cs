using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace ELK
{
    public static class LogsExtensions
    {
        public static IHostBuilder AddElasticSearch(this IHostBuilder host)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ElasticSearchConfig.ConfigureELS(configuration, env))
                .CreateLogger();

            //host.UseSerilog((ctx, lc) => lc
            //    .Enrich.FromLogContext()
            //    .Enrich.WithExceptionDetails()
            //    .WriteTo.Debug()
            //    .WriteTo.Console()
            //    .WriteTo.Elasticsearch(ElasticSearchConfig.ConfigureELS(configuration, env))
            //    .ReadFrom.Configuration(ctx.Configuration));

            host.UseSerilog();
            return host;
        }
    }

    public static class ElasticSearchConfig
    {
        public static ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration, string env)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ELKConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{env.ToLower().Replace(".", ",")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }
    }
}
