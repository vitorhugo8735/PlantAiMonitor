using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PlantDiseaseAI.Api.AI.Configuration;
using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.AI.Models;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Common.Exceptions;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.AI.Inference;

/// <summary>
/// Bridges the C# backend with the Python inference component.
/// </summary>
public sealed class PythonInferenceService : IPlantDiseaseInferenceService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IPythonProcessExecutor _processExecutor;
    private readonly PythonInferenceOptions _options;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger<PythonInferenceService> _logger;

    public PythonInferenceService(
        IPythonProcessExecutor processExecutor,
        IOptions<PythonInferenceOptions> options,
        IHostEnvironment hostEnvironment,
        ILogger<PythonInferenceService> logger)
    {
        _processExecutor = processExecutor;
        _options = options.Value;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    public async Task<InferenceResult> PredictAsync(
        string imagePath,
        CancellationToken cancellationToken = default)
    {
        var resolvedImagePath = ResolvePath(imagePath);
        if (!File.Exists(resolvedImagePath))
        {
            throw new FileNotFoundException($"Image not found: {resolvedImagePath}", resolvedImagePath);
        }

        var pythonProjectRoot = ResolvePath(_options.PythonProjectRoot);
        var scriptPath = ResolvePath(Path.Combine(pythonProjectRoot, _options.InferenceScriptPath));
        var checkpointPath = ResolvePath(Path.Combine(pythonProjectRoot, _options.CheckpointPath));
        var executable = ResolveExecutablePath(_options.PythonExecutablePath);
        var timeout = _options.TimeoutSeconds > 0
            ? TimeSpan.FromSeconds(_options.TimeoutSeconds)
            : TimeSpan.FromSeconds(60);

        _logger.LogInformation(
            "Executing Python inference for image {ImagePath} using script {ScriptPath}.",
            resolvedImagePath,
            scriptPath);

        var processResult = await _processExecutor.ExecuteAsync(
            new PythonProcessRequest
            {
                FileName = executable,
                WorkingDirectory = pythonProjectRoot,
                Arguments =
                [
                    scriptPath,
                    "--image",
                    resolvedImagePath,
                    "--checkpoint-path",
                    checkpointPath,
                    "--model-name",
                    _options.ModelName
                ],
                Timeout = timeout
            },
            cancellationToken);

        if (string.IsNullOrWhiteSpace(processResult.StandardOutput))
        {
            throw new InferenceException("Python inference returned an empty payload.");
        }

        try
        {
            var result = JsonSerializer.Deserialize<InferenceResult>(processResult.StandardOutput, JsonOptions);
            if (result is null)
            {
                throw new InferenceException("Python inference returned an empty JSON payload.");
            }

            _logger.LogInformation(
                "Python inference completed for {ImagePath} with prediction {PredictedClass} ({Confidence:P2}).",
                resolvedImagePath,
                result.PredictedClass,
                result.Confidence);

            return result;
        }
        catch (JsonException exception)
        {
            _logger.LogError(exception, "Python inference returned invalid JSON: {Stdout}", processResult.StandardOutput);
            throw new InferenceException("Python inference returned invalid JSON.", exception);
        }
    }

    private string ResolvePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return _hostEnvironment.ContentRootPath;
        }

        return Path.IsPathRooted(path)
            ? Path.GetFullPath(path)
            : Path.GetFullPath(Path.Combine(_hostEnvironment.ContentRootPath, path));
    }

    private string ResolveExecutablePath(string executable)
    {
        if (string.IsNullOrWhiteSpace(executable))
        {
            throw new InferenceException("The Python executable path is not configured.");
        }

        if (executable.Contains(Path.DirectorySeparatorChar) || executable.Contains(Path.AltDirectorySeparatorChar))
        {
            return ResolvePath(executable);
        }

        return executable;
    }
}