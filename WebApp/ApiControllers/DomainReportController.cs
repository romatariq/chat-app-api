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
    public async Task<DomainReports> GetAll([FromQuery] string url, EDomainReportTimeframe timeframe)
    {
        return await _uow.DomainReportService.GetReports(url, timeframe);
    }

    [HttpPost("{url}")]
    [Authorize]
    public async Task<ActionResult> Add([FromRoute] string url)
    {
        var domainId = await _uow.UrlService.GetOrCreateDomainId(url);
        var userId = User.GetUserId();

        await _uow.DomainReportService.AddReport(domainId, userId);
        await _uow.SaveChangesAsync();

        return Ok();
    }

    [HttpGet("[action]")]
    [Authorize]
    public async Task<bool> CanReport([FromQuery] string url)
    {
        var domainId = await _uow.UrlService.GetOrCreateDomainId(url);
        var userId = User.GetUserId();

        return await _uow.DomainReportService.CanReport(domainId, userId);
    }
}