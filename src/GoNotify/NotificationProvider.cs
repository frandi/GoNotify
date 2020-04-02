using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GoNotify
{
    /// <summary>
    /// The notification provider that will send the message
    /// </summary>
    public abstract class NotificationProvider
    {
        protected NotificationProvider(NotificationProviderOptions options)
        {
            Options = options;

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug).AddConsole();
            });
            Logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        protected NotificationProvider(NotificationProviderOptions options, ILoggerFactory loggerFactory)
        {
            Options = options;
            Logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        /// <summary>
        /// Name of the Notification Provider
        /// </summary>
        public abstract string Name { get; }

        protected NotificationProviderOptions Options { get; }
        protected ILogger Logger { get; }

        /// <summary>
        /// The implementation of Send method that is specific to this provider
        /// </summary>
        /// <param name="messageParameters">The parameters of the message</param>
        /// <returns></returns>
        public abstract Task<NotificationResult> Send(MessageParameterCollection messageParameters);
    }
}
