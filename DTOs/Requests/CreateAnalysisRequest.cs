namespace PlantDiseaseAI.Api.DTOs.Requests;

public sealed class CreateAnalysisRequest
{
    public string RequesterId { get; init; } = string.Empty;

    public string SourceSystem { get; init; } = string.Empty;

    public string CallbackUrl { get; init; } = string.Empty;

    public IFormFile Image { get; init; } = default!;

    public Dictionary<string, string>? Metadata { get; init; }
}