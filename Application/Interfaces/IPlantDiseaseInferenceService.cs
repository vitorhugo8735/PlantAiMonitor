using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Provides single-image inference backed by the Python predictor.
/// </summary>
public interface IPlantDiseaseInferenceService
{
    /// <summary>
    /// Predicts the disease class for a single image path.
    /// </summary>
    /// <param name="imagePath">The image path.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The inference result.</returns>
    Task<InferenceResult> PredictAsync(
        string imagePath,
        CancellationToken cancellationToken = default);
}