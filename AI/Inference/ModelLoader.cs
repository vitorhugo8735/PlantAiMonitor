using PlantDiseaseAI.Api.AI.Interfaces;

namespace PlantDiseaseAI.Api.AI.Inference;

/// <summary>
/// Placeholder model loader that keeps a model instance in memory.
/// </summary>
public sealed class ModelLoader : IModelLoader
{
    private readonly ILogger<ModelLoader> _logger;
    private readonly Lazy<object> _model;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelLoader"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    public ModelLoader(ILogger<ModelLoader> logger)
    {
        _logger = logger;
        _model = new Lazy<object>(CreateModel);
    }

    /// <inheritdoc />
    public bool IsLoaded => _model.IsValueCreated;

    /// <inheritdoc />
    public Task<object> LoadAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_model.Value);
    }

    private object CreateModel()
    {
        _logger.LogInformation("Placeholder model loaded into memory.");
        return new object();
    }
}