using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Contracts.Messages;
using RabbitMQ.Client;

namespace PlantDiseaseAI.Api.Infrastructure.Queue;

/// <summary>
/// Publishes analysis messages to RabbitMQ.
/// </summary>
public sealed class RabbitMqPublisher : IMessagePublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly RabbitMqConnection _connection;
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqPublisher> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqPublisher"/> class.
    /// </summary>
    /// <param name="connection">The shared RabbitMQ connection.</param>
    /// <param name="options">The RabbitMQ options monitor.</param>
    /// <param name="logger">The logger instance.</param>
    public RabbitMqPublisher(
        RabbitMqConnection connection,
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task PublishAnalysisRequestedAsync(
        AnalysisRequestedMessage message,
        CancellationToken cancellationToken = default)
    {
        return PublishAsync(message, QueueNames.AnalysisRequested, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAnalysisCompletedAsync(
        AnalysisCompletedMessage message,
        CancellationToken cancellationToken = default)
    {
        return PublishAsync(message, QueueNames.AnalysisCompleted, cancellationToken);
    }

    /// <inheritdoc />
    public Task PublishAnalysisFailedAsync(
        AnalysisFailedMessage message,
        CancellationToken cancellationToken = default)
    {
        return PublishAsync(message, QueueNames.AnalysisFailed, cancellationToken);
    }

    private Task PublishAsync<T>(T message, string routingKey, CancellationToken cancellationToken)
        where T : class
    {
        var correlationId = GetCorrelationId(message);

        try
        {
            using var channel = _connection.Connection.CreateModel();
            channel.ExchangeDeclare(_options.Exchange, ExchangeType.Direct, durable: true, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, JsonOptions));
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.CorrelationId = correlationId?.ToString();
            properties.ContentType = "application/json";

            channel.BasicPublish(
                exchange: _options.Exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            if (correlationId.HasValue)
            {
                _logger.LogInformation("Message published to RabbitMQ with routing key {RoutingKey}. CorrelationId: {CorrelationId}", routingKey, correlationId);
            }
            else
            {
                _logger.LogInformation("Message published to RabbitMQ with routing key {RoutingKey}.", routingKey);
            }

            return Task.CompletedTask;
        }
        catch (Exception exception)
        {
            if (correlationId.HasValue)
            {
                _logger.LogError(exception, "Failed to publish message to RabbitMQ with routing key {RoutingKey}. CorrelationId: {CorrelationId}", routingKey, correlationId);
            }
            else
            {
                _logger.LogError(exception, "Failed to publish message to RabbitMQ with routing key {RoutingKey}.", routingKey);
            }

            throw;
        }
    }

    private static Guid? GetCorrelationId<T>(T message)
    {
        return message switch
        {
            AnalysisRequestedMessage requested => requested.CorrelationId,
            AnalysisCompletedMessage completed => completed.CorrelationId,
            AnalysisFailedMessage failed => failed.CorrelationId,
            _ => null
        };
    }
}