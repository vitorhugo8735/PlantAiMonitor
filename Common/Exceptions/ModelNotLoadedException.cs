namespace PlantDiseaseAI.Api.Common.Exceptions;

/// <summary>
/// Represents an exception thrown when the model has not been loaded.
/// </summary>
public sealed class ModelNotLoadedException : Exception
{
    public ModelNotLoadedException()
    {
    }

    public ModelNotLoadedException(string message)
        : base(message)
    {
    }

    public ModelNotLoadedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}