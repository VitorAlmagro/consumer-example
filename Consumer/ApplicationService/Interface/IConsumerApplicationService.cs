using System.Threading.Tasks;

namespace Consumer.ApplicationService.Interface
{
    public interface IConsumerApplicationService
    {
        void ProcessMessage(string message);
    }
}
