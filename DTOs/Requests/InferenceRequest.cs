namespace PlantDiseaseAI.Api.DTOs.Requests;

/// <summary>
/// Represents a single-image inference request.
/// </summary>
public sealed class InferenceRequest
{
    public IFormFile Image { get; init; } = default!;
}