using PlantDiseaseAI.Api.AI.Models;

namespace PlantDiseaseAI.Api.AI.Interfaces;

/// <summary>
/// Executes an external Python process in a controlled manner.
/// </summary>
public interface IPythonProcessExecutor
{
    /// <summary>
    /// Runs a Python process and returns captured output.
    /// </summary>
    /// <param name="request">The process request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task<PythonProcessResult> ExecuteAsync(
        PythonProcessRequest request,
        CancellationToken cancellationToken = default);
}