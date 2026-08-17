using MassTransit;
using Microsoft.Extensions.Options;
using PlantDiseaseAI.Api.Messaging.Consumers;

namespace PlantDiseaseAI.Api.Messaging.Configuration;

/// <summary>
/// Configures MassTransit with RabbitMQ.
/// </summary>
public static class MassTransitConfiguration
{
    /// <summary>
    /// Registers MassTransit and its RabbitMQ transport.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMassTransitMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<RabbitMqOptions>()
            .Bind(configuration.GetSection("RabbitMq"))
            .ValidateDataAnnotations();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<AnalysisRequestedConsumer>();

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;

                var virtualHost = options.VirtualHost.TrimStart('/');
                var hostAddress = string.IsNullOrWhiteSpace(virtualHost)
                    ? new Uri($"rabbitmq://{options.Host}:{options.Port}/")
                    : new Uri($"rabbitmq://{options.Host}:{options.Port}/{virtualHost}");

                cfg.Host(hostAddress, hostConfigurator =>
                {
                    hostConfigurator.Username(options.Username);
                    hostConfigurator.Password(options.Password);
                });

                cfg.ReceiveEndpoint(options.QueueName, endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumer<AnalysisRequestedConsumer>(context);
                });
            });
        });

        return services;
    }
}