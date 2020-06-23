using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlightMobileApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            ICommandManager commandManager = new MyCommandManager(Configuration);
            services.AddSingleton(commandManager);
            services.AddSingleton<IScreenshotManager, MyScreenshotManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            /*app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.Value;

                //Redirect to an external URL 
                if (url.Contains("/screenshot"))
                {
                    int httpPort = Configuration.GetValue<int>("SimulatorInfo:HttpPort");
                    context.Response.Redirect("http://10.0.2.2:" + httpPort + "/screenshot");
                    return;
                }
                await next();
            });*/

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
