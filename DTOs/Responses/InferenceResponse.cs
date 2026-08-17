using System.Text.Json.Serialization;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.DTOs.Responses;

/// <summary>
/// Represents the stable JSON response for a single-image inference.
/// </summary>
public sealed class InferenceResponse
{
    [JsonPropertyName("predictedClassId")]
    public int PredictedClassId { get; init; }

    [JsonPropertyName("predictedClass")]
    public string PredictedClass { get; init; } = string.Empty;

    [JsonPropertyName("confidence")]
    public double Confidence { get; init; }

    [JsonPropertyName("probabilities")]
    public Dictionary<string, double> Probabilities { get; init; } = new();

    public static InferenceResponse FromModel(InferenceResult model)
    {
        return new InferenceResponse
        {
            PredictedClassId = model.PredictedClassId,
            PredictedClass = model.PredictedClass,
            Confidence = model.Confidence,
            Probabilities = new Dictionary<string, double>(model.Probabilities)
        };
    }
}