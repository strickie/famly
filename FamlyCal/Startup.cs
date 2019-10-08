using FamlyCal.Clients;
using FamlyCal.OutputFormatters;
using FamlyCal.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FamlyCal
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsProduction())
            {
                services.AddHttpsRedirection(options => options.HttpsPort = 443);
            }
            else
            {
                services.AddHttpsRedirection(options => options.HttpsPort = 5001);
            }

            services
                .AddMvc(options =>
                {
                    options.OutputFormatters.Add(new VCalendarOutputFormatter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpClient();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddHttpClient<FamlyClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
