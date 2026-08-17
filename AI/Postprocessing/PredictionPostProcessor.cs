using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.AI.Models;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.AI.Postprocessing;

/// <summary>
/// Converts Python inference output into the domain prediction model.
/// </summary>
public sealed class PredictionPostProcessor : IPredictionPostProcessor
{
    /// <inheritdoc />
    public Task<PredictionResult> PostProcessAsync(
        InferenceResult prediction,
        string imageReference,
        CancellationToken cancellationToken = default)
    {
        if (prediction is null)
        {
            throw new ArgumentNullException(nameof(prediction));
        }

        if (string.IsNullOrWhiteSpace(prediction.PredictedClass))
        {
            throw new ArgumentException("The prediction does not contain a class name.", nameof(prediction));
        }

        var probabilities = prediction.Probabilities
            .ToDictionary(pair => pair.Key, pair => (float)pair.Value, StringComparer.OrdinalIgnoreCase);

        var rankedPredictions = probabilities
            .Select((pair, index) => new DiseasePrediction
            {
                DiseaseId = index,
                Disease = pair.Key,
                Confidence = pair.Value,
                Probability = pair.Value
            })
            .ToList();

        return Task.FromResult(new PredictionResult
        {
            ImageReference = imageReference,
            PredictedClassId = prediction.PredictedClassId,
            PredictedClass = prediction.PredictedClass,
            Disease = prediction.PredictedClass,
            Confidence = (float)prediction.Confidence,
            Probabilities = probabilities,
            Description = $"Predicted class: {prediction.PredictedClass}",
            ModelVersion = "python-inference",
            ProcessedAt = DateTime.UtcNow,
            Predictions = rankedPredictions
        });
    }
}