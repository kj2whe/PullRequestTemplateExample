﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace luhnAPI
{
    public class Startup
    {
        private readonly ILogger _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddApplicationInsightsTelemetry("b3c8d2d3-7013-40ea-b00c-38d1a0b87645");
            _logger.LogInformation("From ConfigureServices. Services.AddMVC invoked");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseMvc();

        }
    }
}
