using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            .UseStartup<Startup>()
            .Build();
    }
}
