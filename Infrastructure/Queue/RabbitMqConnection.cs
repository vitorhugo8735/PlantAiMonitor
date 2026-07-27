using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace PlantDiseaseAI.Api.Infrastructure.Queue;

/// <summary>
/// Creates and reuses a RabbitMQ connection.
/// </summary>
public sealed class RabbitMqConnection : IDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqConnection> _logger;
    private readonly Lazy<IConnection> _connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="RabbitMqConnection"/> class.
    /// </summary>
    /// <param name="options">The RabbitMQ options monitor.</param>
    /// <param name="logger">The logger instance.</param>
    public RabbitMqConnection(IOptions<RabbitMqOptions> options, ILogger<RabbitMqConnection> logger)
    {
        _options = options.Value;
        _logger = logger;
        _connection = new Lazy<IConnection>(CreateConnection);
    }

    /// <summary>
    /// Gets the shared RabbitMQ connection.
    /// </summary>
    public IConnection Connection => _connection.Value;

    private IConnection CreateConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _options.Host,
            Port = _options.Port,
            UserName = _options.Username,
            Password = _options.Password,
            VirtualHost = _options.VirtualHost,
            DispatchConsumersAsync = true
        };

        var connection = factory.CreateConnection();
        _logger.LogInformation("RabbitMQ connection created to {Host}:{Port} on vhost {VirtualHost}.", _options.Host, _options.Port, _options.VirtualHost);
        return connection;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_connection.IsValueCreated)
        {
            _connection.Value.Dispose();
        }
    }
}