namespace PlantDiseaseAI.Api.Models;

/// <summary>
/// Represents a single disease prediction returned by the model.
/// </summary>
public sealed class DiseasePrediction
{
    /// <summary>
    /// Gets or sets the predicted disease name.
    /// </summary>
    public string Disease { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the confidence score for the prediction.
    /// </summary>
    public float Confidence { get; set; }
}
