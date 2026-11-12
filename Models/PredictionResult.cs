namespace PlantDiseaseAI.Api.Models;

/// <summary>
/// Represents the final output of the inference process.
/// </summary>
public sealed class PredictionResult
{
    /// <summary>
    /// Gets or sets the image reference used for inference.
    /// </summary>
    public string ImageReference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the predicted class identifier.
    /// </summary>
    public int PredictedClassId { get; set; }

    /// <summary>
    /// Gets or sets the predicted class name.
    /// </summary>
    public string PredictedClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the primary disease identified by the model.
    /// </summary>
    public string Disease { get; set; } = string.Empty;

    public bool IsHealthy { get; set; }

    /// <summary>
    /// Gets or sets the confidence score of the prediction.
    /// </summary>
    public float Confidence { get; set; }

    /// <summary>
    /// Gets or sets the textual description of the result.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the model that produced the result.
    /// </summary>
    public string ModelVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the moment when the prediction was processed.
    /// </summary>
    public DateTime ProcessedAt { get; set; }

    /// <summary>
    /// Gets or sets the ranked disease predictions.
    /// </summary>
    public List<DiseasePrediction> Predictions { get; set; } = new();

    /// <summary>
    /// Gets or sets the per-class probabilities.
    /// </summary>
    public Dictionary<string, float> Probabilities { get; set; } = new();
}
