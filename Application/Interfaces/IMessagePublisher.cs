using PlantDiseaseAI.Api.Contracts.Messages;

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
        AnalysisRequestedMessage message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an analysis completed message.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task PublishAnalysisCompletedAsync(
        AnalysisCompletedMessage message,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes an analysis failed message.
    /// </summary>
    /// <param name="message">The message to publish.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task PublishAnalysisFailedAsync(
        AnalysisFailedMessage message,
        CancellationToken cancellationToken = default);
}