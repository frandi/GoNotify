using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoNotify;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly INotification _notification;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            INotification notification,
            IConfiguration configuration)
        {
            _logger = logger;
            _notification = notification;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            await SendEmailWithSmtp(items);
            await SendEmailWithSendGrid(items);

            return items;
        }

        private async Task SendEmailWithSmtp(WeatherForecast[] items)
        {
            var message = new EmailMessage()
            {
                ToAddresses = new List<string> { _configuration.GetValue<string>("SampleUsers") },
                FromAddress = _configuration.GetValue<string>("DefaultFromAddress"),
                Subject = "[GoNotify - SMTP] Weather Forecast",
                Body = $"Forecast: {JsonConvert.SerializeObject(items)}"
            };
            var result = await _notification.SendEmailWithSmtp(message);

            if (result.IsSuccess)
                _logger.LogDebug("Email notification was sent to {toAddresses}", message.ToAddresses);
            else
                _logger.LogWarning("Failed to send notification to {toAddresses}. Error: {error}", message.ToAddresses, result.Errors);
        }

        private async Task SendEmailWithSendGrid(WeatherForecast[] items)
        {
            var htmlContent = "<h1>Forecast</h1>";
            htmlContent += "<ul>";
            foreach (var item in items)
            {
                htmlContent += $"<li>{JsonConvert.SerializeObject(item)}</li>";
            }
            htmlContent += "</ul>";

            var sgMessage = new SendGridMessage()
            {
                ToAddresses = new List<string> { _configuration.GetValue<string>("SampleUsers") },
                FromAddress = _configuration.GetValue<string>("DefaultFromAddress"),
                Subject = "[GoNotify - SendGrid] Weather Forecast",
                PlainContent = $"Forecast: {JsonConvert.SerializeObject(items)}",
                HtmlContent = htmlContent
            };

            var result = await _notification.SendEmailWithSendGrid(sgMessage);

            if (result.IsSuccess)
                _logger.LogDebug("Email notification was sent to {toAddresses}", sgMessage.ToAddresses);
            else
                _logger.LogWarning("Failed to send notification to {toAddresses}. Error: {error}", sgMessage.ToAddresses, result.Errors);
        }
    }
}
