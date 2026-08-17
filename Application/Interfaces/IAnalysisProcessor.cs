namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Orchestrates the complete lifecycle of an analysis.
/// </summary>
public interface IAnalysisProcessor
{
    /// <summary>
    /// Processes a complete analysis flow.
    /// </summary>
    /// <param name="analysisId">The analysis identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ProcessAsync(
        Guid analysisId,
        CancellationToken cancellationToken = default);
}