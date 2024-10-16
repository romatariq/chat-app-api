using System.ComponentModel.DataAnnotations;
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
public class CommentReactionsController: ControllerBase
{
    private readonly IAppBLL _uow;
    private readonly CommentReactionMapper _mapper;

    public CommentReactionsController(IAppBLL uow, IMapper autoMapper)
    {
        _uow = uow;
        _mapper = new CommentReactionMapper(autoMapper);
    }
    
    [HttpPost]
    public async Task<CommentReaction> Add([FromBody] CommentReaction reaction)
    {
        var bllReaction = _mapper.Map(reaction)!;
        bllReaction.UserId = User.GetUserId();
        
        var addedReaction = await _uow.CommentReactionService.Add(bllReaction);
        await _uow.SaveChangesAsync();

        return _mapper.Map(addedReaction)!;
    }
    
    
    [HttpPut]
    public async Task<CommentReaction> Update([FromBody] CommentReaction reaction)
    {
        var bllReaction = _mapper.Map(reaction)!;
        bllReaction.UserId = User.GetUserId();

        var updatedReaction = await _uow.CommentReactionService.Update(bllReaction);
        await _uow.SaveChangesAsync();

        return _mapper.Map(updatedReaction)!;
    }
        
    
    [HttpDelete]
    public async Task<ActionResult> Delete([FromBody] CommentReaction reaction)
    {
        var bllReaction = _mapper.Map(reaction)!;
        bllReaction.UserId = User.GetUserId();

        await _uow.CommentReactionService.Delete(bllReaction);
        await _uow.SaveChangesAsync();
        
        return Ok();
    }
}