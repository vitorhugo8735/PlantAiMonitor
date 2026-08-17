namespace PlantDiseaseAI.Api.Contracts.Events;

/// <summary>
/// Represents a completed analysis notification.
/// </summary>
public sealed class AnalysisCompletedEvent
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
    /// Gets or sets the detected disease.
    /// </summary>
    public string Disease { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the confidence score.
    /// </summary>
    public float Confidence { get; init; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the completion timestamp.
    /// </summary>
    public DateTime FinishedAt { get; init; }
}