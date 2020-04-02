using System.Threading.Tasks;

namespace GoNotify
{
    public static class NotificationExtension
    {
        /// <summary>
        /// Send message with <see cref="SendGridProvider"/>
        /// </summary>
        /// <param name="notification">The notification object</param>
        /// <param name="message">The wrapper of the message that will be sent</param>
        /// <returns></returns>
        public static Task<NotificationResult> SendEmailWithSendGrid(this INotification notification, SendGridMessage message)
        {
            return notification.Send(SendGridConstants.DefaultName, message.ToParameters());
        }
    }
}
