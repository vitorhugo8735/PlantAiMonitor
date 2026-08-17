using System.Text.Json.Serialization;

namespace PlantDiseaseAI.Api.Models;

/// <summary>
/// Represents the stable inference payload returned by the Python component.
/// </summary>
public sealed class InferenceResult
{
    [JsonPropertyName("predictedClassId")]
    public int PredictedClassId { get; set; }

    [JsonPropertyName("predictedClass")]
    public string PredictedClass { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("probabilities")]
    public Dictionary<string, double> Probabilities { get; set; } = new();
}