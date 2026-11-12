using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using PlantDiseaseAI.Api.AI.Configuration;
using PlantDiseaseAI.Api.AI.Inference;
using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.AI.Pipeline;
using PlantDiseaseAI.Api.AI.Postprocessing;
using PlantDiseaseAI.Api.AI.Preprocessing;
using PlantDiseaseAI.Api.Application.Configuration;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Application.Services;
using PlantDiseaseAI.Api.Infrastructure.Repositories;
using PlantDiseaseAI.Api.Infrastructure.Storage;
using PlantDiseaseAI.Api.Infrastructure.Persistence;

namespace PlantDiseaseAI.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<PythonInferenceOptions>(configuration.GetSection("PythonInference"));
        services.Configure<CallbackOptions>(configuration.GetSection("Callback"));

        var connectionString = configuration.GetConnectionString("AnalysisDatabase")
            ?? "Data Source=plant-ai-monitor.db";
        services.AddDbContext<AnalysisDbContext>(options => options.UseSqlite(connectionString));

        services.AddHttpClient<ICallbackService, CallbackService>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<CallbackOptions>>().Value;
            client.Timeout = TimeSpan.FromSeconds(Math.Max(1, options.TimeoutSeconds));
        });

        services.AddScoped<IAnalysisService, AnalysisService>();
        services.AddScoped<IAnalysisProcessor, AnalysisProcessor>();
        services.AddScoped<IAnalysisRepository, SqliteAnalysisRepository>();
        services.AddScoped<IMessagePublisher, PlantDiseaseAI.Api.Messaging.Publishers.MessagePublisher>();
        services.AddSingleton<IModelLoader, ModelLoader>();
        services.AddScoped<IStorageService, LocalStorageService>();
        services.AddScoped<IImagePreprocessor, ImagePreprocessor>();
        services.AddScoped<IModelRunner, ModelRunner>();
        services.AddScoped<IPredictionPostProcessor, PredictionPostProcessor>();
        services.AddScoped<IAnalysisPipeline, AnalysisPipeline>();
        services.AddSingleton<IPythonProcessExecutor, PythonProcessExecutor>();
        services.AddScoped<IPlantDiseaseInferenceService, PythonInferenceService>();

        return services;
    }
}
