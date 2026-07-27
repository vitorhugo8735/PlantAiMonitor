namespace PlantDiseaseAI.Api.Contracts.Messages;

public sealed class AnalysisRequestedMessage
{
    public Guid AnalysisId { get; init; }

    public Guid CorrelationId { get; init; }

    public string RequesterId { get; init; } = string.Empty;

    public string CallbackUrl { get; init; } = string.Empty;

    public string ImagePath { get; init; } = string.Empty;

    public Dictionary<string, string>? Metadata { get; init; }

    public DateTime CreatedAt { get; init; }
}
