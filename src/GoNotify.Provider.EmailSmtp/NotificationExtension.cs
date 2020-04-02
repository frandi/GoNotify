using System.Threading.Tasks;

namespace GoNotify
{
    public static class NotificationExtension
    {
        /// <summary>
        /// Send message through <see cref="EmailSmtpProvider"/>
        /// </summary>
        /// <param name="notification">The notification object</param>
        /// <param name="message">The email message object</param>
        /// <returns></returns>
        public static Task<NotificationResult> SendEmailWithSmtp(this INotification notification, EmailMessage message)
        {
            return notification.Send(EmailSmtpConstants.DefaultName, message.ToParameters());
        }
    }
}
