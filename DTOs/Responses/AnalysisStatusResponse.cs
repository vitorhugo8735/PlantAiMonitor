namespace PlantDiseaseAI.Api.DTOs.Responses;

public sealed class AnalysisStatusResponse
{
    public Guid AnalysisId { get; init; }

    public string Status { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }
}