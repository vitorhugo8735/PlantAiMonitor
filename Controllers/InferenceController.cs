using Microsoft.AspNetCore.Mvc;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Common.Exceptions;
using PlantDiseaseAI.Api.DTOs.Requests;
using PlantDiseaseAI.Api.DTOs.Responses;

namespace PlantDiseaseAI.Api.Controllers;

[ApiController]
[Route("inference")]
public sealed class InferenceController : ControllerBase
{
    private static readonly HashSet<string> SupportedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".bmp",
        ".webp"
    };

    private readonly IPlantDiseaseInferenceService _inferenceService;
    private readonly ILogger<InferenceController> _logger;

    public InferenceController(
        IPlantDiseaseInferenceService inferenceService,
        ILogger<InferenceController> logger)
    {
        _inferenceService = inferenceService;
        _logger = logger;
    }

    [HttpPost("predict")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<InferenceResponse>> PredictAsync(
        [FromForm] InferenceRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Image is null || request.Image.Length == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid image",
                Detail = "An image file is required.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var fileExtension = Path.GetExtension(request.Image.FileName);
        if (string.IsNullOrWhiteSpace(fileExtension) || !SupportedExtensions.Contains(fileExtension))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid image",
                Detail = "The uploaded file extension is not supported.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var tempFilePath = Path.Combine(
            Path.GetTempPath(),
            $"{Guid.NewGuid()}{fileExtension}");

        await using (var fileStream = System.IO.File.Create(tempFilePath))
        {
            await request.Image.CopyToAsync(fileStream, cancellationToken);
        }

        try
        {
            var result = await _inferenceService.PredictAsync(tempFilePath, cancellationToken);
            return Ok(InferenceResponse.FromModel(result));
        }
        catch (FileNotFoundException exception)
        {
            _logger.LogWarning(exception, "Image not found during inference.");
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid image",
                Detail = "The uploaded image could not be found or processed.",
                Status = StatusCodes.Status400BadRequest
            });
        }
        catch (InferenceTimeoutException exception)
        {
            _logger.LogError(exception, "Python inference timed out.");
            return StatusCode(StatusCodes.Status504GatewayTimeout, new ProblemDetails
            {
                Title = "Inference timeout",
                Detail = "The inference process did not complete in time.",
                Status = StatusCodes.Status504GatewayTimeout
            });
        }
        catch (InferenceException exception)
        {
            _logger.LogError(exception, "Python inference failed.");
            return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
            {
                Title = "Inference failed",
                Detail = "The prediction service could not process the image.",
                Status = StatusCodes.Status502BadGateway
            });
        }
        finally
        {
            try
            {
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Failed to delete temporary upload file {TempFilePath}.", tempFilePath);
            }
        }
    }
}