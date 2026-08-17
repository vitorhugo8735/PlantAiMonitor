namespace PlantDiseaseAI.Api.Contracts.Events;

/// <summary>
/// Represents a failed analysis notification.
/// </summary>
public sealed class AnalysisFailedEvent
{
    /// <summary>
    /// Gets or sets the analysis identifier.
    /// </summary>
    public Guid AnalysisId { get; init; }

    /// <summary>
    /// Gets or sets the correlation identifier.
    /// </summary>
    public Guid CorrelationId { get; init; }

    /// <summary>
    /// Gets or sets the requester identifier.
    /// </summary>
    public string RequesterId { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the failure error message.
    /// </summary>
    public string Error { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the failure timestamp.
    /// </summary>
    public DateTime FailedAt { get; init; }
}