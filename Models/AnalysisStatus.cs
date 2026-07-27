namespace PlantDiseaseAI.Api.Models;

/// <summary>
/// Represents the current lifecycle state of an analysis job.
/// </summary>
public enum AnalysisStatus
{
    Pending = 0,
    Queued = 1,
    Processing = 2,
    Completed = 3,
    Failed = 4
}
