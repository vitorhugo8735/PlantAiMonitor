using MassTransit;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Contracts.Commands;
using PlantDiseaseAI.Api.Contracts.Events;

namespace PlantDiseaseAI.Api.Messaging.Publishers;

/// <summary>
/// Publishes analysis messages through MassTransit.
/// </summary>
public sealed class MessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(IPublishEndpoint publishEndpoint, ILogger<MessagePublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishAnalysisRequestedAsync(
        AnalysisRequestedCommand message,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing analysis requested message for {AnalysisId}.", message.AnalysisId);
        await _publishEndpoint.Publish(message, cancellationToken);
    }

    public async Task PublishAnalysisCompletedAsync(
        AnalysisCompletedEvent message,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing analysis completed message for {AnalysisId}.", message.AnalysisId);
        await _publishEndpoint.Publish(message, cancellationToken);
    }

    public async Task PublishAnalysisFailedAsync(
        AnalysisFailedEvent message,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Publishing analysis failed message for {AnalysisId}.", message.AnalysisId);
        await _publishEndpoint.Publish(message, cancellationToken);
    }
}