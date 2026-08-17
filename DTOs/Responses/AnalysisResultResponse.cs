using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.DTOs.Responses;

public sealed class AnalysisResultResponse
{
    public Guid AnalysisId { get; init; }

    public string Status { get; init; } = string.Empty;

    public string ImageReference { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime? StartedAt { get; init; }

    public DateTime? FinishedAt { get; init; }

    public int? PredictedClassId { get; init; }

    public string? PredictedClass { get; init; }

    public string? Disease { get; init; }

    public float? Confidence { get; init; }

    public string? Description { get; init; }

    public Dictionary<string, float>? Probabilities { get; init; }

    public List<DiseasePrediction>? Predictions { get; init; }
}