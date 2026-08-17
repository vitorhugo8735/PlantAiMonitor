using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Common.Exceptions;

namespace PlantDiseaseAI.Api.Infrastructure.Storage;

/// <summary>
/// Stores images locally under the Uploads folder.
/// </summary>
public sealed class LocalStorageService : IStorageService
{
    private static readonly string UploadsRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

    /// <inheritdoc />
    public async Task<string> SaveImageAsync(
        Stream imageStream,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(UploadsRootPath);

        var extension = Path.GetExtension(fileName);
        var generatedFileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(UploadsRootPath, generatedFileName);

        await using var fileStream = File.Create(filePath);
        await imageStream.CopyToAsync(fileStream, cancellationToken);

        return Path.Combine("Uploads", generatedFileName).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    /// <inheritdoc />
    public Task<Stream> GetImageAsync(
        string imageReference,
        CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageReference);

        if (!File.Exists(filePath))
        {
            throw new InvalidImageException($"Image not found: {imageReference}");
        }

        return Task.FromResult<Stream>(File.OpenRead(filePath));
    }

    /// <inheritdoc />
    public Task DeleteImageAsync(
        string imageReference,
        CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), imageReference);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}