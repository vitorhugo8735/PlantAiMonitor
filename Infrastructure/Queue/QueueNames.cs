namespace PlantDiseaseAI.Api.Infrastructure.Queue;

/// <summary>
/// Contains queue names used by the microservice.
/// </summary>
public static class QueueNames
{
    public const string AnalysisRequested = "analysis.requested";

    public const string AnalysisCompleted = "analysis.completed";

    public const string AnalysisFailed = "analysis.failed";
}