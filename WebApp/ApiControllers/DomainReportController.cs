using System.Net.Mime;
using App.Contracts.BLL;
using App.DTO.Common;
using App.DTO.Public.v1;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class DomainReportController: ControllerBase
{
    private readonly IAppBLL _uow;

    public DomainReportController(IAppBLL uow)
    {
        _uow = uow;
    }
    
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(DomainReports), StatusCodes.Status200OK)]
    public async Task<ActionResult<DomainReports>> GetAll([FromQuery] string url, EDomainReportTimeframe timeframe)
    {
        var(domain, _, __) = UrlHelpers.ParseEncodedUrl(url);

        var result = await _uow.DomainReportService.GetReports(domain, timeframe);
        return new DomainReports
        {
            Reports = result,
            Domain = domain
        };
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status201Created)]
    public async Task<ActionResult> Add([FromQuery] string url)
    {
        var userId = User.GetUserId();
        var domainId = await _uow.UrlService.GetOrCreateDomainId(url);

        await _uow.DomainReportService.AddReport(domainId, userId);

        return Ok();
    }
}