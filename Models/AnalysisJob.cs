namespace PlantDiseaseAI.Api.Models;

/// <summary>
/// Represents a complete analysis request handled by the microservice.
/// </summary>
public sealed class AnalysisJob
{
    /// <summary>
    /// Gets or sets the unique analysis identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the correlation identifier used across asynchronous operations.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the requester identifier.
    /// </summary>
    public string RequesterId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source system that initiated the request.
    /// </summary>
    public string SourceSystem { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback URL used to notify the caller.
    /// </summary>
    public string CallbackUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the image reference.
    /// </summary>
    public string ImageReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the metadata attached to the analysis.
    /// </summary>
    public AnalysisMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Gets or sets the inference result.
    /// </summary>
    public PredictionResult? Result { get; set; }

    /// <summary>
    /// Gets or sets the current analysis status.
    /// </summary>
    public AnalysisStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the time when the analysis job was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the time when processing started.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the time when processing finished.
    /// </summary>
    public DateTime? FinishedAt { get; set; }
}
