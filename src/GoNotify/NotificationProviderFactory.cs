using System.Collections.Generic;
using System.Linq;

namespace GoNotify
{
    /// <summary>
    /// The collection of notification provider
    /// </summary>
    public class NotificationProviderFactory : INotificationProviderFactory
    {
        private readonly List<NotificationProvider> _providers;
        private static readonly NotificationProviderFactory _instance = new NotificationProviderFactory();

        public NotificationProviderFactory()
        {
            _providers = new List<NotificationProvider>();
        }

        public static NotificationProviderFactory GetInstance() => _instance;

        public INotificationProviderFactory AddProvider<TProvider>(TProvider provider)
            where TProvider : NotificationProvider, new()
        {
            _providers.Add(provider);

            return this;
        }

        public NotificationProvider GetProvider(string providerName)
        {
            return _providers.FirstOrDefault(p => p.Name == providerName);
        }
    }
}
