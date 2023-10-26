using System.ComponentModel.DataAnnotations;
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
    public async Task<ActionResult<ResponseWithPaging<IEnumerable<VerifyUser>>>> GetAll(
        [FromQuery] [Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 25)
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
            .Skip((pageNr - 1)  * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var usersCount = _userManager.Users
            .Count(u => !admins.Contains(u));

        return new ResponseWithPaging<IEnumerable<VerifyUser>>()
        {
            Data = users,
            PageNr = pageNr,
            PageSize = pageSize,
            PageCount = usersCount / pageSize + (usersCount % pageSize == 0 ? 0 : 1)
        };
    }
    
    
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(VerifyUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VerifyUser>> Set(
        [FromBody] VerifyUser setUser)
    {
        var user = await _userManager.FindByEmailAsync(setUser.Email);
        if (user == null)
        {
            return BadRequest(new ErrorResponse()
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