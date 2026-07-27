namespace PlantDiseaseAI.Api.Application.Interfaces;

/// <summary>
/// Defines storage operations for plant images.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Saves an image stream and returns its reference.
    /// </summary>
    /// <param name="imageStream">The image stream to save.</param>
    /// <param name="fileName">The original file name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The stored image reference.</returns>
    Task<string> SaveImageAsync(
        Stream imageStream,
        string fileName,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an image stream by reference.
    /// </summary>
    /// <param name="imageReference">The stored image reference.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The image stream.</returns>
    Task<Stream> GetImageAsync(
        string imageReference,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an image by reference.
    /// </summary>
    /// <param name="imageReference">The stored image reference.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteImageAsync(
        string imageReference,
        CancellationToken cancellationToken = default);
}