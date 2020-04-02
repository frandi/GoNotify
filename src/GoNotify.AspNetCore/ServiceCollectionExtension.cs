using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace GoNotify
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add GoNotify services
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="actionOptions">The notification options</param>
        /// <returns></returns>
        public static NotificationBuilder AddGoNotify(this IServiceCollection services)
        {
            if (!services.Any(sc => sc.ServiceType == typeof(INotificationProviderFactory)))
                services.AddSingleton<INotificationProviderFactory>(NotificationProviderFactory.GetInstance());

            if (!services.Any(sc => sc.ServiceType == typeof(INotification)))
                services.AddTransient<INotification, Notification>();

            return new NotificationBuilder(services);
        }
    }
}
