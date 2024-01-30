using System.Net.Mime;
using App.Contracts.BLL;
using App.Mappers.AutoMappers.PublicDTO;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PublicV1 = App.DTO.Public.v1;
using Bll = App.DTO.Private.BLL;

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
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PublicV1.CommentReaction), StatusCodes.Status201Created)]
    public async Task<ActionResult<PublicV1.CommentReaction>> Add([FromBody] PublicV1.CommentReaction reaction)
    {
        var bllReaction = new Bll.CommentReaction()
        {
            UserId = User.GetUserId(),
            CommentId = reaction.CommentId,
            ReactionType = reaction.ReactionType
        };
        
        var addedReaction = await _uow.CommentReactionService.Add(bllReaction);
        
        await _uow.SaveChangesAsync();

        return  _mapper.Map(addedReaction)!;
    }
    
    
    [HttpPut]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PublicV1.CommentReaction), StatusCodes.Status200OK)]
    public async Task<ActionResult<PublicV1.CommentReaction>> Update([FromBody] PublicV1.CommentReaction reaction)
    {
        var bllReaction = new Bll.CommentReaction()
        {
            UserId = User.GetUserId(),
            CommentId = reaction.CommentId,
            ReactionType = reaction.ReactionType
        };

        var updatedReaction = await _uow.CommentReactionService.Update(bllReaction);
        
        await _uow.SaveChangesAsync();

        return _mapper.Map(updatedReaction)!;
    }
        
    
    [HttpDelete]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete([FromQuery] Guid commentId)
    {
        await _uow.CommentReactionService.Delete(commentId, User.GetUserId());
        
        await _uow.SaveChangesAsync();
        
        return Ok();
    }
}