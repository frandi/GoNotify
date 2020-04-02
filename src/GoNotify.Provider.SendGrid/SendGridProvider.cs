using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoNotify
{
    /// <summary>
    /// The notification provider to send email via SendGrid
    /// </summary>
    public class SendGridProvider : NotificationProvider
    {
        private readonly SendGridOptions _sendGridOptions;

        /// <summary>
        /// Instantiate the <see cref="SendGridProvider"/>
        /// </summary>
        /// <param name="sendGridOptions">The options for this provider</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SendGridProvider(SendGridOptions sendGridOptions, ILoggerFactory loggerFactory)
            : base(sendGridOptions, loggerFactory)
        {
            _sendGridOptions = sendGridOptions;
        }

        /// <summary>
        /// Instantiate the <see cref="SendGridProvider"/>
        /// </summary>
        /// <param name="sendGridOptions">The options for this provider</param>
        /// <param name="loggerFactory">The logger factory</param>
        public SendGridProvider(IOptions<SendGridOptions> sendGridOptions, ILoggerFactory loggerFactory) 
            : base(sendGridOptions.Value, loggerFactory)
        {
            _sendGridOptions = sendGridOptions.Value;
        }

        /// <inheritdoc cref="NotificationProvider.Name"/>
        public override string Name => SendGridConstants.DefaultName;

        /// <inheritdoc cref="NotificationProvider.Send(MessageParameterCollection)"/>
        public override async Task<NotificationResult> Send(MessageParameterCollection messageParameters)
        {
            var sgMessage = new SendGridMessage(messageParameters);

            var from = new EmailAddress(sgMessage.FromAddress);
            var tos = sgMessage.ToAddresses.Select(to => new EmailAddress(to)).ToList();
            var subject = sgMessage.Subject;
            var plainTextContent = sgMessage.PlainContent;
            var htmlContent = sgMessage.HtmlContent;

            var message = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, plainTextContent, htmlContent);

            var client = new SendGridClient(_sendGridOptions.Apikey);
            var response = await client.SendEmailAsync(message);

            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                return new NotificationResult(true);

            var respBody = await response.Body.ReadAsStringAsync();
            return new NotificationResult(new List<string> { respBody });
        }
    }
}
