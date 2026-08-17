using PlantDiseaseAI.Api.DTOs.Requests;
using PlantDiseaseAI.Api.DTOs.Responses;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Orchestrates the lifecycle of plant disease analyses.
/// </summary>
public interface IAnalysisService
{
    /// <summary>
    /// Creates a new analysis request.
    /// </summary>
    /// <param name="request">The analysis request payload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created analysis response.</returns>
    Task<CreateAnalysisResponse> CreateAsync(
        CreateAnalysisRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the current status of an analysis.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The analysis status, or <c>null</c> when not found.</returns>
    Task<AnalysisStatusResponse?> GetStatusAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the final result of an analysis.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The analysis result, or <c>null</c> when not found.</returns>
    Task<AnalysisResultResponse?> GetResultAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the current status of an analysis.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="status">The new analysis status.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateStatusAsync(
        Guid analysisId,
        AnalysisStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists the result of an analysis.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="result">The prediction result to store.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SaveResultAsync(
        Guid analysisId,
        PredictionResult result,
        CancellationToken cancellationToken = default);
}