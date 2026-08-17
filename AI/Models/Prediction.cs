namespace PlantDiseaseAI.Api.AI.Models;

/// <summary>
/// Represents a raw prediction produced by the model runner.
/// </summary>
public sealed class Prediction
{
    /// <summary>
    /// Gets or sets the predicted class name.
    /// </summary>
    public string ClassName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the plant name.
    /// </summary>
    public string PlantName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the disease name.
    /// </summary>
    public string DiseaseName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confidence score.
    /// </summary>
    public float Confidence { get; set; }

    /// <summary>
    /// Gets or sets the inference execution time.
    /// </summary>
    public TimeSpan InferenceTime { get; set; }

    /// <summary>
    /// Gets or sets the model version used to produce the prediction.
    /// </summary>
    public string ModelVersion { get; set; } = string.Empty;
}