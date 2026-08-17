namespace PlantDiseaseAI.Api.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when an analysis cannot be found.
/// </summary>
public sealed class AnalysisNotFoundException : Exception
{
    public AnalysisNotFoundException()
    {
    }

    public AnalysisNotFoundException(string message)
        : base(message)
    {
    }

    public AnalysisNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}