namespace PlantDiseaseAI.Api.AI.Interfaces;

/// <summary>
/// Defines image preprocessing operations.
/// </summary>
public interface IImagePreprocessor
{
    /// <summary>
    /// Preprocesses an image stream into model input data.
    /// </summary>
    /// <param name="imageStream">The image stream to preprocess.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The preprocessed tensor data.</returns>
    Task<float[]> PreprocessAsync(
        Stream imageStream,
        CancellationToken cancellationToken = default);
}