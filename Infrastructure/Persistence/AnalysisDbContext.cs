using Microsoft.EntityFrameworkCore;

namespace PlantDiseaseAI.Api.Infrastructure.Persistence;

public sealed class AnalysisDbContext : DbContext
{
    public AnalysisDbContext(DbContextOptions<AnalysisDbContext> options)
        : base(options)
    {
    }

    public DbSet<AnalysisRecord> Analyses => Set<AnalysisRecord>();
}

public sealed class AnalysisRecord
{
    public Guid Id { get; set; }

    public string Payload { get; set; } = string.Empty;
}