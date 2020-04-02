using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GoNotify
{
    /// <summary>
    /// The implementation of <see cref="EmailSmtpProvider"/>
    /// </summary>
    public class EmailSmtpProvider : NotificationProvider
    {
        private readonly SmtpOptions _smtpOptions;

        /// <summary>
        /// Instantiate the <see cref="EmailSmtpProvider"/>
        /// </summary>
        /// <param name="smtpOptions">The SMTP options</param>
        /// <param name="loggerFactory">The logger factory</param>
        public EmailSmtpProvider(SmtpOptions smtpOptions, ILoggerFactory loggerFactory)
            : base(smtpOptions, loggerFactory)
        {
            _smtpOptions = smtpOptions;
        }

        /// <summary>
        /// Instantiate the <see cref="EmailSmtpProvider"/>
        /// </summary>
        /// <param name="smtpOptions">The SMTP options</param>
        /// <param name="loggerFactory">The logger factory</param>
        public EmailSmtpProvider(IOptions<SmtpOptions> smtpOptions, ILoggerFactory loggerFactory)
            : base(smtpOptions.Value, loggerFactory)
        {
            _smtpOptions = smtpOptions.Value;
        }

        /// <summary>
        /// <inheritdoc cref="NotificationProvider.Name"/>
        /// </summary>
        public override string Name => EmailSmtpConstants.DefaultName;

        /// <summary>
        /// <inheritdoc cref="NotificationProvider.Send(MessageParameterCollection)"/>
        /// </summary>
        public override async Task<NotificationResult> Send(MessageParameterCollection messageParameters)
        {
            var emailMessage = new EmailMessage(messageParameters);

            // mail message
            var message = new MailMessage
            {
                From = new MailAddress(emailMessage.FromAddress),
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = emailMessage.IsHtml
            };

            foreach (var address in emailMessage.ToAddresses)
            {
                message.To.Add(new MailAddress(address));
            }

            foreach (var address in emailMessage.CCAddresses)
            {
                message.CC.Add(new MailAddress(address));
            }

            foreach (var address in emailMessage.BCCAddresses)
            {
                message.Bcc.Add(new MailAddress(address));
            }

            // smtp client
            var smtpClient = new SmtpClient(_smtpOptions.Server, _smtpOptions.Port)
            {
                EnableSsl = _smtpOptions.UseSSL
            };

            if (!string.IsNullOrEmpty(_smtpOptions.Username))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password);
            }

            // send email
            Logger.LogDebug("Sending email {subject} to {toAddresses}", message.Subject, message.To.Select(to => to.Address));

            await smtpClient.SendMailAsync(message);

            Logger.LogDebug("Email {subject} has been sent to {toAddresses}", message.Subject, message.To.Select(to => to.Address));

            return new NotificationResult(true);
        }
    }
}
