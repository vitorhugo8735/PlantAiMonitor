using Microsoft.AspNetCore.Mvc;
using PlantDiseaseAI.Api.Application.Interfaces;
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
    public async Task<ActionResult<CreateAnalysisResponse>> CreateAsync([FromForm] CreateAnalysisRequest request)
    {
        var response = await _analysisService.CreateAsync(request);
        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AnalysisStatusResponse>> GetStatusAsync(Guid id)
    {
        var response = await _analysisService.GetStatusAsync(id);
        return response is null ? NotFound() : Ok(response);
    }

    [HttpGet("result/{id:guid}")]
    public async Task<ActionResult<AnalysisResultResponse>> GetResultAsync(Guid id)
    {
        var response = await _analysisService.GetResultAsync(id);
        return response is null ? NotFound() : Ok(response);
    }
}