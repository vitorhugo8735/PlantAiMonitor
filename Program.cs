
using PlantDiseaseAI.Api.Extensions;
using PlantDiseaseAI.Api.Messaging.Extensions;
using PlantDiseaseAI.Api.Infrastructure.Persistence;

namespace PlantDiseaseAI.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddMessagingInfrastructure(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        using (var scope = app.Services.CreateScope())
        {
            var database = scope.ServiceProvider.GetRequiredService<AnalysisDbContext>();
            database.Database.EnsureCreated();
        }

        app.Run();
    }
}
