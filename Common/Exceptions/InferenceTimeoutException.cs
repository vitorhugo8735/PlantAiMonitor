namespace PlantDiseaseAI.Api.Common.Exceptions;

/// <summary>
/// Represents a timeout while waiting for Python inference to complete.
/// </summary>
public sealed class InferenceTimeoutException : InferenceException
{
    public InferenceTimeoutException()
    {
    }

    public InferenceTimeoutException(string message)
        : base(message)
    {
    }

    public InferenceTimeoutException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}