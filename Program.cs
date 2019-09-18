using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace luhnAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var host = BuildWebHost(args);
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("From Program. Running the host now.."); // This will be picked up by AI
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseApplicationInsights()
            .UseStartup<Startup>()
            .ConfigureLogging(logging =>
            {
                // Optional: Apply filters to configure LogLevel Trace or above is sent to
                // ApplicationInsights for all categories.
                logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);

                // Additional filtering For category starting in "Microsoft",
                // only Warning or above will be sent to Application Insights.
                logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Trace);
            })
            .Build();
    }
}
