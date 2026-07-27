namespace PlantDiseaseAI.Api.Contracts.Messages;

public sealed class AnalysisFailedMessage
{
    public Guid AnalysisId { get; init; }

    public Guid CorrelationId { get; init; }

    public string RequesterId { get; init; } = string.Empty;

    public string Error { get; init; } = string.Empty;

    public DateTime FailedAt { get; init; }
}
