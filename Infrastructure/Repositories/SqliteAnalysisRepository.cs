using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Infrastructure.Persistence;
using PlantDiseaseAI.Api.Models;

namespace PlantDiseaseAI.Api.Infrastructure.Repositories;

public sealed class SqliteAnalysisRepository : IAnalysisRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly AnalysisDbContext _database;

    public SqliteAnalysisRepository(AnalysisDbContext database)
    {
        _database = database;
    }

    public async Task AddAsync(AnalysisJob job, CancellationToken cancellationToken = default)
    {
        _database.Analyses.Add(ToRecord(job));
        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task<AnalysisJob?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = await _database.Analyses
            .AsNoTracking()
            .SingleOrDefaultAsync(item => item.Id == id, cancellationToken);

        return record is null ? null : FromRecord(record);
    }

    public async Task<IEnumerable<AnalysisJob>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _database.Analyses.AsNoTracking().ToListAsync(cancellationToken);
        return records.Select(FromRecord).ToArray();
    }

    public async Task UpdateAsync(AnalysisJob job, CancellationToken cancellationToken = default)
    {
        var record = await _database.Analyses
            .SingleOrDefaultAsync(item => item.Id == job.Id, cancellationToken);

        if (record is null)
        {
            _database.Analyses.Add(ToRecord(job));
        }
        else
        {
            record.Payload = JsonSerializer.Serialize(job, JsonOptions);
        }

        await _database.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = await _database.Analyses.SingleOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (record is null)
        {
            return;
        }

        _database.Analyses.Remove(record);
        await _database.SaveChangesAsync(cancellationToken);
    }

    private static AnalysisRecord ToRecord(AnalysisJob job)
    {
        return new AnalysisRecord
        {
            Id = job.Id,
            Payload = JsonSerializer.Serialize(job, JsonOptions)
        };
    }

    private static AnalysisJob FromRecord(AnalysisRecord record)
    {
        return JsonSerializer.Deserialize<AnalysisJob>(record.Payload, JsonOptions)
            ?? throw new InvalidOperationException($"Analysis record '{record.Id}' contains invalid data.");
    }
}