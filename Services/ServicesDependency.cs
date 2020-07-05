using Microsoft.Extensions.DependencyInjection;

namespace WorkSampleExperis.Services
{
    public static class ServicesDependency
    {
        public static void AddServiceDependency(this IServiceCollection services)
        {
            services.AddTransient<IConvertService, ConvertService>();
        }
    }
}