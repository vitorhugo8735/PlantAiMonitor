using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Defines HTTP callback operations for analysis outcomes.
/// </summary>
public interface ICallbackService
{
    /// <summary>
    /// Sends a successful analysis result to the caller callback URL.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="callbackUrl">The callback URL.</param>
    /// <param name="result">The analysis result.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendResultAsync(
        Guid analysisId,
        string callbackUrl,
        PredictionResult result,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a failure notification to the caller callback URL.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="callbackUrl">The callback URL.</param>
    /// <param name="error">The failure description.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendFailureAsync(
        Guid analysisId,
        string callbackUrl,
        string error,
        CancellationToken cancellationToken = default);
}