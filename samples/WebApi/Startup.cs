using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoNotify;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApi
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

            services
                .AddGoNotify()
                .AddProvider<EmailSmtpProvider, SmtpOptions>(options =>
                {
                    options.Server = Configuration.GetValue<string>("Smtp:Server");
                    options.Port = Configuration.GetValue<int>("Smtp:Port");
                    options.Username = Configuration.GetValue<string>("Smtp:Username");
                    options.Password = Configuration.GetValue<string>("Smtp:Password");
                    options.UseSSL = Configuration.GetValue<bool>("Smtp:UseSSL");
                });
                //.AddProvider<SlackProvider, SlackOptions>(options =>
                //{
                //    options.Organization = Configuration.GetValue<string>("Slack:Organization");
                //    options.ClientId = Configuration.GetValue<string>("Slack:ClientId");
                //    options.ClientSecret = Configuration.GetValue<int>("Slack:ClientSecret");
                //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
