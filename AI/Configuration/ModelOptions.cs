namespace PlantDiseaseAI.Api.AI.Configuration;

/// <summary>
/// Represents the future AI model configuration.
/// </summary>
public sealed class ModelOptions
{
    /// <summary>
    /// Gets or sets the model file path.
    /// </summary>
    public string ModelPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expected image width.
    /// </summary>
    public int InputWidth { get; set; }

    /// <summary>
    /// Gets or sets the expected image height.
    /// </summary>
    public int InputHeight { get; set; }

    /// <summary>
    /// Gets or sets the minimum confidence threshold.
    /// </summary>
    public float ConfidenceThreshold { get; set; }

    /// <summary>
    /// Gets or sets the device used for inference.
    /// </summary>
    public string Device { get; set; } = "cpu";

    /// <summary>
    /// Gets or sets a value indicating whether half precision should be used.
    /// </summary>
    public bool UseHalfPrecision { get; set; }
}