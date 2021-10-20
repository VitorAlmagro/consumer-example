using Consumer.DependencyInjection.ApplicationService;
using Microsoft.Extensions.DependencyInjection;

namespace Consumer.DependencyInjection
{
    public static class ConfigureBindingsDependencyInjection
    {
        public static void DependencyInjection(this IServiceCollection services)
        {
            ConfigureBindingsApplicationService.RegisterBindings(services);
        }
    }
}
