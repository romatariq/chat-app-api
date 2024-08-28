using System.Net.Mime;
using App.Contracts.BLL;
using App.DTO.Public.v1;
using App.Mappers.AutoMappers.PublicDTO;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GroupsController: ControllerBase
{
    private readonly IAppBLL _uow;
    private readonly GroupMapper _mapper;

    public GroupsController(IAppBLL uow, IMapper autoMapper)
    {
        _uow = uow;
        _mapper = new GroupMapper(autoMapper);
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<Group>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Group>>> GetAll()
    {
        Guid? userId = User.IsAuthenticated() ? User.GetUserId() : null;

        var groups = await _uow.GroupService.GetAll(userId);
        return groups.Select(g => _mapper.Map(g)!).ToList();
    }
}