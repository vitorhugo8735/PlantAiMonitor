using PlantDiseaseAI.Api.Messaging.Configuration;

namespace PlantDiseaseAI.Api.Messaging.Extensions;

/// <summary>
/// Extension methods for messaging infrastructure registration.
/// </summary>
public static class MessagingExtensions
{
    /// <summary>
    /// Adds messaging infrastructure to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessagingInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddMassTransitMessaging(configuration);
    }
}