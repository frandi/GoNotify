namespace GoNotify
{
    public interface INotificationProviderFactory
    {
        INotificationProviderFactory AddProvider<TProvider>(TProvider provider) 
            where TProvider : NotificationProvider, new();
        NotificationProvider GetProvider(string providerName);
    }
}
