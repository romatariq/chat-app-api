using System.Net.Mime;
using App.DAL.EF;
using App.DTO.Public.v1;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class GroupsController: ControllerBase
{
    private readonly AppDbContext _ctx;

    public GroupsController(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(IEnumerable<Group>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Group>>> GetAll()
    {
        var userId = User.GetUserId();

        return await _ctx.GroupUsers
            .Include(gu => gu.Group)
            .Where(gu => gu.UserId == userId)
            .Select(gu => new Group
            {
                Id = gu.GroupId,
                Name = gu.Group!.Name,
                Type = gu.Group.GroupType,
                IsOwner = gu.IsOwner
            })
            .ToListAsync();
    }
}