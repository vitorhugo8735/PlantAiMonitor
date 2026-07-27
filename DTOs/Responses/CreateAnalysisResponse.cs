namespace PlantDiseaseAI.Api.DTOs.Responses;

public sealed class CreateAnalysisResponse
{
    public Guid AnalysisId { get; init; }

    public string Status { get; init; } = string.Empty;
}