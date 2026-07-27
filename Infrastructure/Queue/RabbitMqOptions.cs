using System.ComponentModel.DataAnnotations;

namespace PlantDiseaseAI.Api.Infrastructure.Queue;

/// <summary>
/// Represents RabbitMQ configuration settings.
/// </summary>
public sealed class RabbitMqOptions
{
    /// <summary>
    /// Gets or sets the RabbitMQ host.
    /// </summary>
    [Required]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RabbitMQ port.
    /// </summary>
    [Range(1, 65535)]
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the RabbitMQ username.
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RabbitMQ password.
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RabbitMQ virtual host.
    /// </summary>
    [Required]
    public string VirtualHost { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the RabbitMQ exchange name.
    /// </summary>
    [Required]
    public string Exchange { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default queue name.
    /// </summary>
    [Required]
    public string Queue { get; set; } = string.Empty;
}