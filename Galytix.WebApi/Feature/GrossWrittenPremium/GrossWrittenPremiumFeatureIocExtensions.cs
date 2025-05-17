using System.Reflection;

namespace Galytix.WebApi.Feature.GrossWrittenPremium;

public static class GrossWrittenPremiumFeatureIocExtensions
{
    private static readonly Assembly AssemblyToScan = typeof(GrossWrittenPremiumHandler).Assembly;

    public static IServiceCollection AddGrossWrittenPremiumFeature(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyToScan));
        return services;
    }
}