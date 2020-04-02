using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoNotify
{
    public class NotificationBuilder
    {
        public NotificationBuilder(IServiceCollection services) => Services = services;

        public virtual IServiceCollection Services { get; }

        public NotificationBuilder AddProvider<TProvider, TOptions>(Action<TOptions> configureOptions)
            where TProvider: NotificationProvider, new()
            where TOptions: NotificationProviderOptions, new()
        {
            Services.Configure(configureOptions);
            Services.AddTransient<TProvider>();
            //Services.AddSingleton(sp =>
            //{
            //    var factory = sp.GetService<INotificationProviderFactory>();
            //    var provider = sp.GetService<TProvider>();

            //    return factory.AddProvider(provider);

            //});


            return this;
        }
    }
}
