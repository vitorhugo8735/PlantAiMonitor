using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlantDiseaseAI.Api.Application.Configuration;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Services;

public sealed class CallbackService : ICallbackService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CallbackService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    public CallbackService(
        HttpClient httpClient,
        IOptions<CallbackOptions> options,
        ILogger<CallbackService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        _httpClient.Timeout = TimeSpan.FromSeconds(Math.Max(1, options.Value.TimeoutSeconds));
    }

    public async Task SendResultAsync(
        Guid analysisId,
        string callbackUrl,
        PredictionResult result,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(callbackUrl))
        {
            _logger.LogInformation("No callback URL configured for analysis {AnalysisId}. Skipping callback.", analysisId);
            return;
        }

        try
        {
            var payload = new
            {
                analysisId,
                status = AnalysisStatus.Completed.ToString(),
                predictedClassId = result.PredictedClassId,
                predictedClass = result.PredictedClass,
                confidence = result.Confidence,
                probabilities = result.Probabilities,
                imageReference = result.ImageReference
            };

            await SendAsync(callbackUrl, payload, cancellationToken, analysisId, "success");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send analysis callback for {AnalysisId} to {CallbackUrl}.", analysisId, callbackUrl);
        }
    }

    public async Task SendFailureAsync(
        Guid analysisId,
        string callbackUrl,
        string error,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(callbackUrl))
        {
            _logger.LogInformation("No callback URL configured for analysis {AnalysisId}. Skipping failure callback.", analysisId);
            return;
        }

        try
        {
            var payload = new
            {
                analysisId,
                status = AnalysisStatus.Failed.ToString(),
                error
            };

            await SendAsync(callbackUrl, payload, cancellationToken, analysisId, "failure");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to send analysis failure callback for {AnalysisId} to {CallbackUrl}.", analysisId, callbackUrl);
        }
    }

    private async Task SendAsync(
        string callbackUrl,
        object payload,
        CancellationToken cancellationToken,
        Guid analysisId,
        string callbackType)
    {
        if (!Uri.TryCreate(callbackUrl, UriKind.Absolute, out var endpoint))
        {
            _logger.LogWarning("Invalid callback URL for analysis {AnalysisId}: {CallbackUrl}", analysisId, callbackUrl);
            return;
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = JsonContent.Create(payload, payload.GetType(), options: _jsonOptions)
        };

        request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        _logger.LogInformation("Sending analysis {CallbackType} callback for {AnalysisId} to {CallbackUrl}", callbackType, analysisId, callbackUrl);

        using var response = await _httpClient.SendAsync(request, cancellationToken);

        if ((int)response.StatusCode >= 200 && (int)response.StatusCode < 300)
        {
            _logger.LogInformation("Analysis {CallbackType} callback sent successfully for {AnalysisId}.", callbackType, analysisId);
            return;
        }

        _logger.LogWarning(
            "Analysis {CallbackType} callback for {AnalysisId} returned {StatusCode}.",
            callbackType,
            analysisId,
            (int)response.StatusCode);
    }
}
