using Consumer.ApplicationService.Interface;
using System;
using System.Threading.Tasks;

namespace Consumer.ApplicationService
{
    public class ConsumerApplicationService : IConsumerApplicationService
    {
        public void ProcessMessage(string message)
        {
            Console.WriteLine($"MESSAGE {message}");
        }
    }
}
