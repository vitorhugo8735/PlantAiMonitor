using PlantDiseaseAI.Api.AI.Interfaces;

namespace PlantDiseaseAI.Api.AI.Preprocessing;

/// <summary>
/// Placeholder implementation of image preprocessing.
/// </summary>
public sealed class ImagePreprocessor : IImagePreprocessor
{
    /// <inheritdoc />
    public Task<float[]> PreprocessAsync(
        Stream imageStream,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Array.Empty<float>());
    }
}