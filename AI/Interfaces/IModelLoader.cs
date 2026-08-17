namespace PlantDiseaseAI.Api.AI.Interfaces;

/// <summary>
/// Defines a loader for the trained model instance.
/// </summary>
public interface IModelLoader
{
    /// <summary>
    /// Gets a value indicating whether the model has been loaded.
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    /// Loads the model into memory if needed.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A placeholder model instance.</returns>
    Task<object> LoadAsync(CancellationToken cancellationToken = default);
}