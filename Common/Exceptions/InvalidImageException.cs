namespace PlantDiseaseAI.Api.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when an image is invalid.
/// </summary>
public sealed class InvalidImageException : Exception
{
    public InvalidImageException()
    {
    }

    public InvalidImageException(string message)
        : base(message)
    {
    }

    public InvalidImageException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}