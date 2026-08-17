using PlantDiseaseAI.Api.Contracts.Commands;
using PlantDiseaseAI.Api.Contracts.Events;

namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Defines message publishing operations.
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes an analysis requested message.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task PublishAnalysisRequestedAsync(
        AnalysisRequestedCommand message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an analysis completed message.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task PublishAnalysisCompletedAsync(
        AnalysisCompletedEvent message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an analysis failed message.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task PublishAnalysisFailedAsync(
        AnalysisFailedEvent message,
        CancellationToken cancellationToken = default);
}