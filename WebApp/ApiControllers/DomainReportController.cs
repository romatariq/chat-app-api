using System.Net.Mime;
using App.Contracts.BLL;
using App.DTO.Common;
using App.DTO.Public.v1;
using App.Helpers;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
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

    [HttpPost("{url}")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status201Created)]
    public async Task<ActionResult> Add([FromRoute] string url)
    {
        var domainId = await _uow.UrlService.GetOrCreateDomainId(UrlHelpers.ParseEncodedUrl(url).domain);
        var userId = User.GetUserId();

        await _uow.DomainReportService.AddReport(domainId, userId);
        await _uow.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("[action]")]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> CanReport([FromQuery] string url)
    {
        var domainId = await _uow.UrlService.GetOrCreateDomainId(UrlHelpers.ParseEncodedUrl(url).domain);
        var userId = User.GetUserId();

        return await _uow.DomainReportService.CanReport(domainId, userId);
    }
}