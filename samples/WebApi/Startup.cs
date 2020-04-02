using GoNotify;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                })
                .AddProvider<SendGridProvider, SendGridOptions>(options =>
                {
                    options.Apikey = Configuration.GetValue<string>("SendGrid:ApiKey");
                });
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
