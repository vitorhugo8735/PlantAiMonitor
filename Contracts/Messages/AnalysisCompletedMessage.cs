namespace PlantDiseaseAI.Api.Contracts.Messages;

public sealed class AnalysisCompletedMessage
{
    public Guid AnalysisId { get; init; }

    public Guid CorrelationId { get; init; }

    public string RequesterId { get; init; } = string.Empty;

    public string Disease { get; init; } = string.Empty;

    public float Confidence { get; init; }

    public string Description { get; init; } = string.Empty;

    public DateTime FinishedAt { get; init; }
}
