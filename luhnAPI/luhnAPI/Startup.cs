using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace luhnAPI
{
    public class Startup
    {
        private readonly ILogger _logger;
        private readonly string _instrumentationKey;

        //public Startup(IConfiguration configuration, ILogger<Startup> logger)
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
            _instrumentationKey = Configuration.GetSection("ApplicationInsights:InstrumentationKey").Value;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = true;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = true;
                    options.SuppressMapClientErrors = true;
                    options.ClientErrorMapping[404].Link =
                        "https://httpstatuses.com/404";
                });
            //services.AddMvcCore().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            services.AddLogging(builder =>
            {
                builder.AddApplicationInsights(_instrumentationKey);
                builder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
                builder.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Error);
            });

            services.AddApplicationInsightsTelemetry();


            _logger.LogInformation("From ConfigureServices. Services.AddMVC invoked");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                _logger.LogInformation("Configuring for Development environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                _logger.LogInformation("Configuring for Production environment");
                app.UseHsts();
            }

            app.UseCors(
                options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
