using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.AI.Models;
using PlantDiseaseAI.Api.Common.Exceptions;

namespace PlantDiseaseAI.Api.AI.Inference;

/// <summary>
/// Placeholder implementation of the model runner.
/// </summary>
public sealed class ModelRunner : IModelRunner
{
    private readonly IModelLoader _modelLoader;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelRunner"/> class.
    /// </summary>
    /// <param name="modelLoader">The model loader.</param>
    public ModelRunner(IModelLoader modelLoader)
    {
        _modelLoader = modelLoader;
    }

    /// <inheritdoc />
    public Task<Prediction> RunAsync(
        float[] input,
        CancellationToken cancellationToken = default)
    {
        return RunInternalAsync(cancellationToken);
    }

    private async Task<Prediction> RunInternalAsync(CancellationToken cancellationToken)
    {
        var model = await _modelLoader.LoadAsync(cancellationToken);

        if (model is null || !_modelLoader.IsLoaded)
        {
            throw new ModelNotLoadedException("The model could not be loaded into memory.");
        }

        return new Prediction
        {
            ClassName = "Tomato___Healthy",
            PlantName = "Tomato",
            DiseaseName = "Healthy",
            Confidence = 0.99f,
            InferenceTime = TimeSpan.Zero,
            ModelVersion = "placeholder"
        };
    }
}