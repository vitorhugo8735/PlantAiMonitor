namespace PlantDiseaseAI.Api.DTOs.Responses;

public sealed class AnalysisResultResponse
{
    public Guid AnalysisId { get; init; }

    public string Status { get; init; } = string.Empty;

    public string? Disease { get; init; }

    public float? Confidence { get; init; }

    public string? Description { get; init; }
}