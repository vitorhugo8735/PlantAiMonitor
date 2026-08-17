using PlantDiseaseAI.Api.AI.Models;

namespace PlantDiseaseAI.Api.AI.Interfaces;

/// <summary>
/// Defines model execution operations.
/// </summary>
public interface IModelRunner
{
    /// <summary>
    /// Runs inference against preprocessed input data.
    /// </summary>
    /// <param name="input">The preprocessed input data.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The raw model prediction.</returns>
    Task<Prediction> RunAsync(
        float[] input,
        CancellationToken cancellationToken = default);
}