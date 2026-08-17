using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.AI.Pipeline;

/// <summary>
/// Analysis pipeline that orchestrates Python inference and post-processing.
/// </summary>
public sealed class AnalysisPipeline : IAnalysisPipeline
{
    private readonly IPlantDiseaseInferenceService _inferenceService;
    private readonly IPredictionPostProcessor _postProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisPipeline"/> class.
    /// </summary>
    /// <param name="inferenceService">The inference service.</param>
    /// <param name="postProcessor">The prediction post-processor.</param>
    public AnalysisPipeline(
        IPlantDiseaseInferenceService inferenceService,
        IPredictionPostProcessor postProcessor)
    {
        _inferenceService = inferenceService;
        _postProcessor = postProcessor;
    }

    /// <inheritdoc />
    public async Task<PredictionResult> ExecuteAsync(
        string imageReference,
        CancellationToken cancellationToken = default)
    {
        var prediction = await _inferenceService.PredictAsync(imageReference, cancellationToken);
        return await _postProcessor.PostProcessAsync(prediction, imageReference, cancellationToken);
    }
}