using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.AI.Interfaces;

/// <summary>
/// Defines the analysis pipeline orchestration.
/// </summary>
public interface IAnalysisPipeline
{
    /// <summary>
    /// Executes the inference and post-processing steps for a stored image reference.
    /// </summary>
    /// <param name="imageReference">The stored image reference.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The final prediction result.</returns>
    Task<PredictionResult> ExecuteAsync(
        string imageReference,
        CancellationToken cancellationToken = default);
}