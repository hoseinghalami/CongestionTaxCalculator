using Application.Services;
using Application.Services.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITaxService, TaxService>();

        return services;
    }
}