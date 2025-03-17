using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

namespace SecureCommSvc.API
{
    public static class SerilogElkLogging
    {
        public const string HealthCheck = @"Executed DbCommand ({elapsed}ms) [Parameters=[{parameters}], CommandType='{commandType}', CommandTimeout='{commandTimeout}']{newLine}{commandText}";
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogger => (hostingContext, loggerConfiguration) =>
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            loggerConfiguration
                .Filter.ByExcluding("RequestPath like '/health%'")
                .Filter.ByExcluding("RequestPath like '/swagger%'")
                .Filter.ByExcluding("SourceContext like 'Microsoft.AspNetCore.DataProtection.KeyManagement.XmlKeyManager'")
                .Filter.ByExcluding(logEvent => logEvent.MessageTemplate.Text == HealthCheck)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithCorrelationIdHeader("X-Correlation-Id")
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                .WriteTo.File($"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-log.txt")
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration);

            var url = configuration["ElasticConfiguration:Uri"];
            var appName = configuration["ElasticConfiguration:AppName"];

            if (!string.IsNullOrWhiteSpace(appName))
            {
                loggerConfiguration.Enrich.WithProperty("ApplicationName", appName);
            }

            //if (!string.IsNullOrWhiteSpace(url))
            //{
            //    loggerConfiguration.WriteTo.Elasticsearch(
            //        new ElasticsearchSinkOptions(new Uri(url))
            //        {
            //            MinimumLogEventLevel = LogEventLevel.Information,
            //            AutoRegisterTemplate = true,
            //            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            //            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy.MM.dd}"
            //        });
            //}
        };
    }
}
