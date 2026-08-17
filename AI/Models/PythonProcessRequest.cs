namespace PlantDiseaseAI.Api.AI.Models;

/// <summary>
/// Describes a Python process invocation.
/// </summary>
public sealed class PythonProcessRequest
{
    public string FileName { get; init; } = string.Empty;

    public string WorkingDirectory { get; init; } = string.Empty;

    public IReadOnlyList<string> Arguments { get; init; } = Array.Empty<string>();

    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(60);
}