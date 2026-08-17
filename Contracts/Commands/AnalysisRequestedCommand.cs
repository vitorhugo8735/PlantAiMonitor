namespace PlantDiseaseAI.Api.Contracts.Commands;

/// <summary>
/// Represents a request to start an analysis.
/// </summary>
public sealed class AnalysisRequestedCommand
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
    /// Gets or sets the callback URL.
    /// </summary>
    public string CallbackUrl { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the image path.
    /// </summary>
    public string ImagePath { get; init; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional metadata.
    /// </summary>
    public Dictionary<string, string>? Metadata { get; init; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; init; }
}