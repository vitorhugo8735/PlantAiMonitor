namespace PlantDiseaseAI.Api.Common.Exceptions;

/// <summary>
/// Represents an exception raised while invoking Python inference.
/// </summary>
public class InferenceException : Exception
{
    public InferenceException()
    {
    }

    public InferenceException(string message)
        : base(message)
    {
    }

    public InferenceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}