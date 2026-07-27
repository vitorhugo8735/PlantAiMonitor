using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Application.Services;
using PlantDiseaseAI.Api.Infrastructure.Repositories;
using PlantDiseaseAI.Api.Infrastructure.Queue;
using PlantDiseaseAI.Api.Infrastructure.Storage;
using Microsoft.Extensions.Options;

namespace PlantDiseaseAI.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddSingleton<IAnalysisRepository, InMemoryAnalysisRepository>();
        services.AddScoped<IStorageService, LocalStorageService>();
        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();

        services.AddSingleton<RabbitMqConnection>();
        services.AddOptions<RabbitMqOptions>()
            .BindConfiguration("RabbitMq")
            .ValidateDataAnnotations();

        return services;
    }
}
