using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RepositoryLayer.Helper
{
    public class Publisher
        {
            private readonly ILogger<Publisher> _logger;
            private readonly IConfiguration _configuration;
            public Publisher(ILogger<Publisher> logger,IConfiguration configuration)
            {
                _logger = logger;
                _configuration = configuration;
            }
            public void PublishToQueue(string queueName, string message)
        {
            try
            {
                _logger.LogInformation("publisher is running");
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    UserName = "guest",
                    Password = "guest"
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                _logger.LogInformation($" [x] Sent message to in consumer {queueName}: {message}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error publishing to RabbitMQ: {ex.Message}");
            }
        }

    }
}
