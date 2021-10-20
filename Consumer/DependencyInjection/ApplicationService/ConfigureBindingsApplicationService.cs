using Consumer.ApplicationService;
using Consumer.ApplicationService.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer.DependencyInjection.ApplicationService
{
    public static class ConfigureBindingsApplicationService
    {
        public static void RegisterBindings(IServiceCollection services)
        {
            services.AddSingleton<IConsumerApplicationService, ConsumerApplicationService>();
        }
    }
}
