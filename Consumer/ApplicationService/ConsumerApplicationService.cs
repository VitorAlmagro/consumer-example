using Consumer.ApplicationService.Interface;
using Microsoft.Extensions.Logging;

namespace Consumer.ApplicationService
{
    public class ConsumerApplicationService : IConsumerApplicationService
    {
        private readonly ILogger<ConsumerApplicationService> _log;

        public ConsumerApplicationService(ILogger<ConsumerApplicationService> log)
        {
            _log = log;
        }

        public void ProcessMessage(string message)
        {
            _log.LogInformation($"Message received: {message}");
        }
    }
}
