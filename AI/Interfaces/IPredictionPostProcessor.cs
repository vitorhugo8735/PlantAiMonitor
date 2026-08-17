using PlantDiseaseAI.Api.AI.Models;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.AI.Interfaces;

/// <summary>
/// Defines prediction post-processing operations.
/// </summary>
public interface IPredictionPostProcessor
{
    /// <summary>
    /// Converts a raw Python inference result into the API result model.
    /// </summary>
    /// <param name="prediction">The raw inference result to post-process.</param>
    /// <param name="imageReference">The stored image reference.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The processed prediction result.</returns>
    Task<PredictionResult> PostProcessAsync(
        InferenceResult prediction,
        string imageReference,
        CancellationToken cancellationToken = default);
}