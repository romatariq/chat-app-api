using System.Net.Mime;
using App.Domain.Identity;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;

[ApiController]
[Authorize]
[Authorize(Roles = "admin")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class VerificationController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public VerificationController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(VerifyUser), StatusCodes.Status200OK)]
    public async Task<ActionResult<RestApiResponseWithPaging<IEnumerable<VerifyUser>>>> GetAll(
        [FromBody] RestApiRequestWithPaging requestWithPaging)
    {
        var admins = await _userManager.GetUsersInRoleAsync("admin");
        
        var users = await _userManager.Users
            .OrderByDescending(u => u.CreatedAtUtc)
            .Where(u => !admins.Contains(u))
            .Select(u => new VerifyUser
            {
                Email = u.Email!,
                IsVerified = u.IsVerified
            })
            .Skip((requestWithPaging.PageNr - 1)  * requestWithPaging.PageSize)
            .Take(requestWithPaging.PageSize)
            .ToListAsync();
        
        var usersCount = _userManager.Users
            .Count(u => !admins.Contains(u));

        return new RestApiResponseWithPaging<IEnumerable<VerifyUser>>()
        {
            Data = users,
            PageNr = requestWithPaging.PageNr,
            PageSize = requestWithPaging.PageSize,
            PageCount = usersCount / requestWithPaging.PageSize + (usersCount % requestWithPaging.PageSize == 0 ? 0 : 1)
        };
    }
    
    
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(VerifyUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyUser>> Set(
        [FromBody] VerifyUser setUser)
    {
        var user = await _userManager.FindByEmailAsync(setUser.Email);
        if (user == null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Error = "User with given email not found",
            });
        }
        
        if (user.IsVerified != setUser.IsVerified)
        {
            user.IsVerified = setUser.IsVerified;
            await _userManager.UpdateAsync(user);
        }
        
        return setUser;
    }
}