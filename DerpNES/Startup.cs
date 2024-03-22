using Microsoft.Extensions.DependencyInjection;

namespace DerpNES;

public static class Startup
{
    public static IServiceCollection AddDerpNES(this IServiceCollection services)
    {
        services.AddTransient<Emulator>();

        return services;
    }
}
