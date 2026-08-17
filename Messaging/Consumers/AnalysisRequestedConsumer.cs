using MassTransit;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Contracts.Commands;

namespace PlantDiseaseAI.Api.Messaging.Consumers;

/// <summary>
/// Consumes analysis requested messages and forwards them to the analysis processor.
/// </summary>
public sealed class AnalysisRequestedConsumer : IConsumer<AnalysisRequestedCommand>
{
    private readonly IAnalysisProcessor _analysisProcessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisRequestedConsumer"/> class.
    /// </summary>
    /// <param name="analysisProcessor">The analysis processor.</param>
    public AnalysisRequestedConsumer(IAnalysisProcessor analysisProcessor)
    {
        _analysisProcessor = analysisProcessor;
    }

    /// <inheritdoc />
    public async Task Consume(ConsumeContext<AnalysisRequestedCommand> context)
    {
        await _analysisProcessor.ProcessAsync(context.Message.AnalysisId, context.CancellationToken);
    }
}