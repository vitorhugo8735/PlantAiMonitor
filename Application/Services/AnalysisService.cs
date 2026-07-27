using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.DTOs.Requests;
using PlantDiseaseAI.Api.DTOs.Responses;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Application.Services;

/// <summary>
/// Provides analysis orchestration backed by the in-memory repository.
/// </summary>
public sealed class AnalysisService : IAnalysisService
{
    private readonly IAnalysisRepository _repository;
    private readonly IStorageService _storageService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisService"/> class.
    /// </summary>
    /// <param name="repository">The analysis repository.</param>
    public AnalysisService(IAnalysisRepository repository, IStorageService storageService)
    {
        _repository = repository;
        _storageService = storageService;
    }

    /// <inheritdoc />
    public async Task<CreateAnalysisResponse> CreateAsync(CreateAnalysisRequest request)
    {
        var analysisId = Guid.NewGuid();
        var correlationId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        var imageReference = await _storageService.SaveImageAsync(request.Image.OpenReadStream(), request.Image.FileName);

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
            Status = AnalysisStatus.Pending,
            CreatedAt = createdAt,
            StartedAt = null,
            FinishedAt = null
        };

        await _repository.AddAsync(analysisJob);

        return new CreateAnalysisResponse
        {
            AnalysisId = analysisId,
            Status = analysisJob.Status.ToString()
        };
    }

    /// <inheritdoc />
    public async Task<AnalysisStatusResponse?> GetStatusAsync(Guid id)
    {
        var analysisJob = await _repository.GetByIdAsync(id);
        if (analysisJob is null)
        {
            return null;
        }

        return new AnalysisStatusResponse
        {
            AnalysisId = analysisJob.Id,
            Status = analysisJob.Status.ToString(),
            CreatedAt = analysisJob.CreatedAt
        };
    }

    /// <inheritdoc />
    public async Task<AnalysisResultResponse?> GetResultAsync(Guid id)
    {
        var analysisJob = await _repository.GetByIdAsync(id);
        if (analysisJob is null)
        {
            return null;
        }

        return new AnalysisResultResponse
        {
            AnalysisId = analysisJob.Id,
            Status = analysisJob.Status.ToString(),
            Disease = analysisJob.Result?.Disease,
            Confidence = analysisJob.Result?.Confidence,
            Description = analysisJob.Result?.Description
        };
    }

    /// <inheritdoc />
    public async Task UpdateStatusAsync(Guid analysisId, AnalysisStatus status)
    {
        var analysisJob = await _repository.GetByIdAsync(analysisId);
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

        await _repository.UpdateAsync(analysisJob);
    }

    /// <inheritdoc />
    public async Task SaveResultAsync(Guid analysisId, PredictionResult result)
    {
        var analysisJob = await _repository.GetByIdAsync(analysisId);
        if (analysisJob is null)
        {
            return;
        }

        analysisJob.Result = result;
        analysisJob.Status = AnalysisStatus.Completed;
        analysisJob.FinishedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(analysisJob);
    }
}