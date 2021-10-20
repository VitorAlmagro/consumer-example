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
                using (var server = new ResponseSocket("@tcp://localhost:5556")) // bind
                using (var client = new RequestSocket(">tcp://localhost:5556"))  // connect
                {
                    // Send a message from the client socket
                    client.SendFrame("Hello");

                    // Receive the message from the server socket
                    string m1 = server.ReceiveFrameString();
                    Console.WriteLine();

                    _consumerApplicationService.ProcessMessage($"From Client: {m1}");

                    // Send a response back from the server
                    server.SendFrame("Hi Back");

                    // Receive the response from the client socket
                    string m2 = client.ReceiveFrameString();
                    _consumerApplicationService.ProcessMessage($"From Server: {m2}");
                }
            }
            catch (Exception ex)
            {
                _log.LogError($"Error: {ex.Message}");
            }
        }
    }
}
