using System.Net;
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
    public async Task<ActionResult<IEnumerable<VerifyUser>>> GetAll()
    {
        // TODO: Add paging
        var admins = await _userManager.GetUsersInRoleAsync("admin");
        return await _userManager.Users
            .OrderByDescending(u => u.CreatedAtUtc)
            .Where(u => !admins.Contains(u))
            .Select(u => new VerifyUser
            {
                Email = u.Email!,
                IsVerified = u.IsVerified
            })
            .ToListAsync();
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