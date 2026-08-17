using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Common.Exceptions;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Services;

/// <summary>
/// Orchestrates the full analysis flow using repository, pipeline and callback abstractions.
/// </summary>
public sealed class AnalysisProcessor : IAnalysisProcessor
{
    private readonly IAnalysisRepository _repository;
    private readonly IAnalysisPipeline _pipeline;
    private readonly ICallbackService _callbackService;
    private readonly IAnalysisService _analysisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisProcessor"/> class.
    /// </summary>
    /// <param name="repository">The analysis repository.</param>
    /// <param name="pipeline">The analysis pipeline.</param>
    /// <param name="callbackService">The callback service.</param>
    /// <param name="analysisService">The analysis service.</param>
    public AnalysisProcessor(
        IAnalysisRepository repository,
        IAnalysisPipeline pipeline,
        ICallbackService callbackService,
        IAnalysisService analysisService)
    {
        _repository = repository;
        _pipeline = pipeline;
        _callbackService = callbackService;
        _analysisService = analysisService;
    }

    /// <inheritdoc />
    public async Task ProcessAsync(Guid analysisId, CancellationToken cancellationToken = default)
    {
        var job = await _repository.GetByIdAsync(analysisId, cancellationToken);
        if (job is null)
        {
            throw new AnalysisNotFoundException($"Analysis job '{analysisId}' was not found.");
        }

        try
        {
            await _analysisService.UpdateStatusAsync(analysisId, AnalysisStatus.Processing, cancellationToken);

            var predictionResult = await _pipeline.ExecuteAsync(job.ImageReference, cancellationToken);

            await _analysisService.SaveResultAsync(analysisId, predictionResult, cancellationToken);

            await _analysisService.UpdateStatusAsync(analysisId, AnalysisStatus.Completed, cancellationToken);

            await _callbackService.SendResultAsync(analysisId, job.CallbackUrl, predictionResult, cancellationToken);
        }
        catch (Exception exception)
        {
            await MarkAsFailedAsync(analysisId, job, exception, cancellationToken);
            throw;
        }
    }

    private async Task MarkAsFailedAsync(
        Guid analysisId,
        AnalysisJob job,
        Exception exception,
        CancellationToken cancellationToken)
    {
        try
        {
            await _analysisService.UpdateStatusAsync(analysisId, AnalysisStatus.Failed, cancellationToken);
        }
        catch
        {
        }

        try
        {
            await _callbackService.SendFailureAsync(analysisId, job.CallbackUrl, exception.Message, cancellationToken);
        }
        catch
        {
        }
    }
}