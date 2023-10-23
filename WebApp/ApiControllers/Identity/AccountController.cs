using System.Net.Mime;
using App.DAL.EF;
using App.Domain.Identity;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.Identity;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly Random _rnd = new();
    private readonly AppDbContext _context;


    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
        _context = context;
    }
    
    
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType( StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody] Register registrationData)
    {
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Error = $"Email {registrationData.Email} already registered"
            });
        }
        
        appUser = await _context.Users
            .FirstOrDefaultAsync(u => string.Equals(u.UserName, registrationData.UserName, StringComparison.CurrentCultureIgnoreCase));
        if (appUser != null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Error = $"Username {registrationData.UserName} already registered"
            });
        }

        appUser = new AppUser()
        {
            Email = registrationData.Email,
            UserName = registrationData.UserName
        };

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Error = result.Errors.First().Description
            });
        }
        
        _logger.LogInformation("Created new user. Username: {}, email: {}", registrationData.UserName, registrationData.Email);
        return Ok();
    }

    
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> LogIn([FromBody] Login loginData)
    {
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            await Task.Delay(_rnd.Next(100, 1000));
        
            return BadRequest(new RestApiErrorResponse()
            {
                Error = "User/Password problem"
            });
        }
        
        if (!appUser.IsVerified)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Error = "User is not verified"
            });
        }

        var result = await _signInManager.PasswordSignInAsync(appUser, loginData.Password, true, false);
        if (!result.Succeeded)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            return BadRequest(new RestApiErrorResponse()
            {
                Error = "User/Password problem"
            });
        }

        _logger.LogInformation("{} logged in.", appUser.UserName);
        return Ok();
    }
    
    
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Logout()
    {
        var userId = User.GetUserId();

        var appUser = await _context.Users
            .Where(u => u.Id == userId)
            .SingleOrDefaultAsync();
        if (appUser == null)
        {
            return BadRequest(new RestApiErrorResponse()
            {
                Error = "User problem"
            });
        }

        await _signInManager.SignOutAsync();
        _logger.LogInformation("{} logged out.", appUser.UserName);
        return Ok();
    }


}