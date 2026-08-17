namespace PlantDiseaseAI.Api.AI.Models;

/// <summary>
/// Captures the output of a Python process execution.
/// </summary>
public sealed class PythonProcessResult
{
    public int ExitCode { get; init; }

    public string StandardOutput { get; init; } = string.Empty;

    public string StandardError { get; init; } = string.Empty;
}