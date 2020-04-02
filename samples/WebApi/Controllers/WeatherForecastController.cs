﻿using System;
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

            var message = new EmailMessage()
            {
                ToAddresses = new List<string> { _configuration.GetValue<string>("SampleUsers") },
                FromAddress = _configuration.GetValue<string>("DefaultFromAddress"),
                Subject = "[GoNotify] Weather Forecast",
                Body = $"Forecast: {JsonConvert.SerializeObject(items)}"
            };
            var result = await _notification.SendEmailWithSmtp(message);

            if (result.IsSuccess)
                _logger.LogDebug("Email notification was sent to {toAddresses}", message.ToAddresses);
            else
                _logger.LogWarning("Failed to send notification to {toAddresses}. Error: {error}", message.ToAddresses, result.Errors);

            return items;
        }
    }
}
