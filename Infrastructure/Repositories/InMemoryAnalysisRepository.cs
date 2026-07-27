using System.Collections.Concurrent;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Infrastructure.Repositories;

/// <summary>
/// In-memory repository for analysis jobs.
/// </summary>
public sealed class InMemoryAnalysisRepository : IAnalysisRepository
{
    private readonly ConcurrentDictionary<Guid, AnalysisJob> _analysisJobs = new();

    /// <inheritdoc />
    public Task AddAsync(AnalysisJob job, CancellationToken cancellationToken = default)
    {
        _analysisJobs[job.Id] = job;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<AnalysisJob?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _analysisJobs.TryGetValue(id, out var job);
        return Task.FromResult(job);
    }

    /// <inheritdoc />
    public Task<IEnumerable<AnalysisJob>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<AnalysisJob> jobs = _analysisJobs.Values.ToArray();
        return Task.FromResult(jobs);
    }

    /// <inheritdoc />
    public Task UpdateAsync(AnalysisJob job, CancellationToken cancellationToken = default)
    {
        _analysisJobs[job.Id] = job;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _analysisJobs.TryRemove(id, out _);
        return Task.CompletedTask;
    }
}