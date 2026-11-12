using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.DTOs.Requests;
using PlantDiseaseAI.Api.DTOs.Responses;
using PlantDiseaseAI.Api.Contracts.Commands;
using PlantDiseaseAI.Api.Models;
using PlantDiseaseAI.Api.Common.Exceptions;

namespace PlantDiseaseAI.Api.Application.Services;

/// <summary>
/// Provides analysis orchestration backed by the in-memory repository.
/// </summary>
public sealed class AnalysisService : IAnalysisService
{
    private readonly IAnalysisRepository _repository;
    private readonly IStorageService _storageService;
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<AnalysisService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisService"/> class.
    /// </summary>
    /// <param name="repository">The analysis repository.</param>
    public AnalysisService(
        IAnalysisRepository repository,
        IStorageService storageService,
        IMessagePublisher messagePublisher,
        ILogger<AnalysisService> logger)
    {
        _repository = repository;
        _storageService = storageService;
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<CreateAnalysisResponse> CreateAsync(
        CreateAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Image is null || request.Image.Length == 0)
        {
            throw new InvalidImageException("An image file is required.");
        }

        var analysisId = Guid.NewGuid();
        var correlationId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        await using var imageStream = request.Image.OpenReadStream();
        var imageReference = await _storageService.SaveImageAsync(
            imageStream,
            request.Image.FileName,
            cancellationToken);

        var analysisJob = new AnalysisJob
        {
            Id = analysisId,
            CorrelationId = correlationId,
            RequesterId = request.RequesterId,
            SourceSystem = request.SourceSystem,
            CallbackUrl = request.CallbackUrl,
            ImageReference = imageReference,
            Metadata = new AnalysisMetadata
            {
                AdditionalData = request.Metadata is null
                    ? new Dictionary<string, string>()
                    : new Dictionary<string, string>(request.Metadata)
            },
            Result = null,
            Status = AnalysisStatus.Queued,
            CreatedAt = createdAt,
            StartedAt = null,
            FinishedAt = null
        };

        await _repository.AddAsync(analysisJob, cancellationToken);

        await _messagePublisher.PublishAnalysisRequestedAsync(
            new AnalysisRequestedCommand
            {
                AnalysisId = analysisJob.Id,
                CorrelationId = analysisJob.CorrelationId,
                RequesterId = analysisJob.RequesterId,
                CallbackUrl = analysisJob.CallbackUrl,
                ImagePath = analysisJob.ImageReference,
                Metadata = analysisJob.Metadata.AdditionalData,
                CreatedAt = analysisJob.CreatedAt
            },
            cancellationToken);

        _logger.LogInformation("Analysis {AnalysisId} queued for processing.", analysisId);

        return new CreateAnalysisResponse
        {
            AnalysisId = analysisId,
            Status = analysisJob.Status.ToString()
        };
    }

    /// <inheritdoc />
    public async Task<AnalysisStatusResponse?> GetStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var analysisJob = await _repository.GetByIdAsync(id, cancellationToken);
        if (analysisJob is null)
        {
            return null;
        }

        return new AnalysisStatusResponse
        {
            AnalysisId = analysisJob.Id,
            Status = analysisJob.Status.ToString(),
            CreatedAt = analysisJob.CreatedAt,
            StartedAt = analysisJob.StartedAt,
            FinishedAt = analysisJob.FinishedAt,
            ImageReference = analysisJob.ImageReference
        };
    }

    /// <inheritdoc />
    public async Task<AnalysisResultResponse?> GetResultAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var analysisJob = await _repository.GetByIdAsync(id, cancellationToken);
        if (analysisJob is null)
        {
            return null;
        }

        return new AnalysisResultResponse
        {
            AnalysisId = analysisJob.Id,
            Status = analysisJob.Status.ToString(),
            PredictedClass = analysisJob.Result?.PredictedClass,
            Disease = analysisJob.Result?.Disease,
            IsHealthy = analysisJob.Result?.IsHealthy,
            Confidence = analysisJob.Result?.Confidence,
            Description = analysisJob.Result?.Description,
            Probabilities = analysisJob.Result?.Probabilities,
            Predictions = analysisJob.Result?.Predictions
                .Select(prediction => new AnalysisPredictionResponse
                {
                    Name = prediction.Disease,
                    Confidence = prediction.Confidence,
                    Description = analysisJob.Result.Description
                })
                .ToList()
        };
    }

    /// <inheritdoc />
    public async Task UpdateStatusAsync(
        Guid analysisId,
        AnalysisStatus status,
        CancellationToken cancellationToken = default)
    {
        var analysisJob = await _repository.GetByIdAsync(analysisId, cancellationToken);
        if (analysisJob is null)
        {
            return;
        }

        analysisJob.Status = status;

        if (status == AnalysisStatus.Processing && analysisJob.StartedAt is null)
        {
            analysisJob.StartedAt = DateTime.UtcNow;
        }

        if (status is AnalysisStatus.Completed or AnalysisStatus.Failed)
        {
            analysisJob.FinishedAt = DateTime.UtcNow;
        }

        await _repository.UpdateAsync(analysisJob, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SaveResultAsync(
        Guid analysisId,
        PredictionResult result,
        CancellationToken cancellationToken = default)
    {
        var analysisJob = await _repository.GetByIdAsync(analysisId, cancellationToken);
        if (analysisJob is null)
        {
            return;
        }

        analysisJob.Result = result;
        analysisJob.Status = AnalysisStatus.Completed;
        analysisJob.FinishedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(analysisJob, cancellationToken);
    }
}