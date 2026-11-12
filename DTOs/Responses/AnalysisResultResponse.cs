namespace PlantDiseaseAI.Api.DTOs.Responses;

public sealed class AnalysisResultResponse
{
    public Guid AnalysisId { get; init; }

    public string Status { get; init; } = string.Empty;

    public string? PredictedClass { get; init; }

    public string? Disease { get; init; }

    public bool? IsHealthy { get; init; }

    public float? Confidence { get; init; }

    public string? Description { get; init; }

    public Dictionary<string, float>? Probabilities { get; init; }

    public List<AnalysisPredictionResponse>? Predictions { get; init; }
}

public sealed class AnalysisPredictionResponse
{
    public string Name { get; init; } = string.Empty;

    public float Confidence { get; init; }

    public string Description { get; init; } = string.Empty;
}