using System.Net.Mime;
using App.Domain.Exceptions;
using App.Domain.Identity;
using App.DTO.Public.v1;
using App.DTO.Public.v1.Identity;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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


    public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }
    
    
    [HttpPost]
    public async Task<ActionResult> Register([FromBody] Register registrationData)
    {
        if (registrationData.Password != registrationData.ConfirmPassword)
        {
            throw new CustomUserBadInputException("Password and confirm password must match.");
        }
        
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            throw new CustomUserBadInputException($"Email {registrationData.Email} already registered.");

        }

        appUser = await _userManager.FindByNameAsync(registrationData.UserName);
        if (appUser != null)
        {
            throw new CustomUserBadInputException($"Username {registrationData.UserName} already registered.");
        }

        appUser = new AppUser()
        {
            Email = registrationData.Email,
            UserName = registrationData.UserName
        };

        var result = await _userManager.CreateAsync(appUser, registrationData.Password);
        if (!result.Succeeded)
        {
            throw new CustomUserBadInputException(result.Errors.First().Description);
        }
        
        await _signInManager.SignInAsync(appUser, true);
        
        _logger.LogInformation("New user registered. Username: {}, email: {}", registrationData.UserName, registrationData.Email);
        return Ok();
    }

    
    [HttpPost]
    public async Task<ActionResult> LogIn([FromBody] Login loginData)
    {
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            throw new CustomUserBadInputException("User/Password problem");
        }

        var result = await _signInManager.PasswordSignInAsync(appUser, loginData.Password, true, false);
        if (!result.Succeeded)
        {
            await Task.Delay(_rnd.Next(100, 1000));
            throw new CustomUserBadInputException("User/Password problem");
        }

        _logger.LogInformation("{} logged in.", appUser.UserName);
        return Ok();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("{} logged out.", User.GetUsername());
        return Ok();
    }

    [HttpGet]
    [Authorize]
    public Task<ActionResult> IsLoggedIn()
    {
        _logger.LogInformation("{} checked if still logged in.", User.GetUsername());
        return Task.FromResult<ActionResult>(Ok());
    }
}