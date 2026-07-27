namespace PlantDiseaseAI.Api.Models;

/// <summary>
/// Represents additional information provided together with the image.
/// </summary>
public sealed class AnalysisMetadata
{
    /// <summary>
    /// Gets or sets the plant species.
    /// </summary>
    public string? PlantSpecies { get; set; }

    /// <summary>
    /// Gets or sets the crop or culture name.
    /// </summary>
    public string? Culture { get; set; }

    /// <summary>
    /// Gets or sets the location information.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets additional arbitrary metadata values.
    /// </summary>
    public Dictionary<string, string>? AdditionalData { get; set; } = new();
}
