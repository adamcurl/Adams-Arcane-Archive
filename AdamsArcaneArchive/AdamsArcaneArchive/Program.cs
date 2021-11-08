using AdamsArcaneArchive.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Display;
using System;

namespace AdamsArcaneArchive
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss}] [{SourceContext}] [{Level:u3}] {Message:lj} {NewLine}{Exception}")
                .WriteTo.File("./Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.File(new RenderedCompactJsonFormatter(), "./Logs/log.ndjson", rollingInterval: RollingInterval.Day)
                .WriteTo.Sentry(o =>
                {
                    o.TextFormatter = new MessageTemplateTextFormatter("{Message}", null);
                })
                .CreateLogger();

            try
            {
                Log.Information("Booting App...");
                CreateHostBuilder(args)
                    .Build()
                    .Migrate(Log.Logger)
                    .Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseSentry();
                    webBuilder.UseStartup<Startup>();
                });
    }
}
