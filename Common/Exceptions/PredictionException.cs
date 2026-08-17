namespace PlantDiseaseAI.Api.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when a prediction cannot be produced.
/// </summary>
public sealed class PredictionException : Exception
{
    public PredictionException()
    {
    }

    public PredictionException(string message)
        : base(message)
    {
    }

    public PredictionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}