using System;
using System.Threading;
using System.Threading.Tasks;
using Consumer.ApplicationService.Interface;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;

namespace Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _log;
        private readonly IConsumerApplicationService _consumerApplicationService;

        public Worker(ILogger<Worker> log, IConsumerApplicationService consumerApplicationService)
        {
            _log = log;
            _consumerApplicationService = consumerApplicationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _log.LogInformation("Iniciando Worker Example");

            try
            {
                string topic = "";
                string connectionString = "tcp://localhost:12345";

                _log.LogInformation($"Subscriber started for Topic : {topic}");

                using (var subSocket = new SubscriberSocket())
                {
                    subSocket.Options.ReceiveHighWatermark = 1000;
                    subSocket.Connect(connectionString);
                    subSocket.Subscribe(topic);

                    _log.LogInformation("Subscriber socket connecting...");

                    while (true)
                    {
                        string messageTopicReceived = subSocket.ReceiveFrameString();
                        string messageReceived = subSocket.ReceiveFrameString();
                        _consumerApplicationService.ProcessMessage(messageReceived);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error: {ex.Message}");
            }
        }
    }
}
