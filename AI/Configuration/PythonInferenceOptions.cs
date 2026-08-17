namespace PlantDiseaseAI.Api.AI.Configuration;

/// <summary>
/// Configuration for the external Python inference component.
/// </summary>
public sealed class PythonInferenceOptions
{
    /// <summary>
    /// Gets or sets the Python executable path or command name.
    /// </summary>
    public string PythonExecutablePath { get; set; } = "python";

    /// <summary>
    /// Gets or sets the Python project root that contains the inference module.
    /// </summary>
    public string PythonProjectRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative path to the Python inference CLI.
    /// </summary>
    public string InferenceScriptPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the relative or absolute checkpoint path.
    /// </summary>
    public string CheckpointPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the model name to request from the Python predictor.
    /// </summary>
    public string ModelName { get; set; } = "efficientnet_b0";

    /// <summary>
    /// Gets or sets the process timeout in seconds.
    /// </summary>
    public int TimeoutSeconds { get; set; } = 60;
}