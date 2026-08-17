using Microsoft.AspNetCore.Mvc;
using PlantDiseaseAI.Api.Application.Interfaces;
using PlantDiseaseAI.Api.Common.Exceptions;
using PlantDiseaseAI.Api.DTOs.Requests;
using PlantDiseaseAI.Api.DTOs.Responses;

namespace PlantDiseaseAI.Api.Controllers;

[ApiController]
[Route("analysis")]
public sealed class AnalysisController : ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateAnalysisResponse>> CreateAsync(
        [FromForm] CreateAnalysisRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _analysisService.CreateAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (InvalidImageException exception)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid image",
                Detail = exception.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnalysisStatusResponse>> GetStatusAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _analysisService.GetStatusAsync(id, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpGet("result/{id:guid}")]
    public async Task<ActionResult<AnalysisResultResponse>> GetResultAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await _analysisService.GetResultAsync(id, cancellationToken);
        return response is null ? NotFound() : Ok(response);
    }
}