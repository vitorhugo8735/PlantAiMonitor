using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Defines persistence operations for analysis jobs.
/// </summary>
public interface IAnalysisRepository
{
    /// <summary>
    /// Adds a new analysis job.
    /// </summary>
    /// <param name="job">The analysis job to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddAsync(
        AnalysisJob job,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an analysis job by its identifier.
    /// </summary>
    /// <param name="id">The analysis job identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The analysis job, or <c>null</c> when not found.</returns>
    Task<AnalysisJob?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all analysis jobs.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>All stored analysis jobs.</returns>
    Task<IEnumerable<AnalysisJob>> GetAllAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing analysis job.
    /// </summary>
    /// <param name="job">The analysis job to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task UpdateAsync(
        AnalysisJob job,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an analysis job by its identifier.
    /// </summary>
    /// <param name="id">The analysis job identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}