using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using App.Contracts.BLL;
using App.DTO.Common;
using App.DTO.Private.Shared;
using App.DTO.Public.v1;
using App.Mappers.AutoMappers.PublicDTO;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Comment = App.DTO.Public.v1.Comment;

namespace WebApp.ApiControllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly IAppBLL _uow;
    private readonly CommentMapper _commentMapper;

    public CommentsController(IAppBLL uow, IMapper autoMapper)
    {
        _uow = uow;
        _commentMapper = new CommentMapper(autoMapper);
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseWithPaging<IEnumerable<Comment>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResponseWithPaging<IEnumerable<Comment>>>> GetAll(
        [FromQuery] string url,
        [FromQuery] Guid groupId,
        [FromQuery] [Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 25,
        [FromQuery]  ESort sort = ESort.Top)
    {
        var userId = User.GetUserId();

        var userInGroup = await _uow.GroupService.IsUserInGroup(userId, groupId);
        if (!userInGroup)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Invalid user/group."
            });
        }

        var (domain, path, parameters) = UrlHelpers.ParseEncodedUrl(url);

        var (comments, pageCount) = await _uow.CommentService
                .GetAll(new GetAllCommentsParameters(groupId, userId, domain, path, parameters, sort, pageNr, pageSize));

        return new ResponseWithPaging<IEnumerable<Comment>>
        {
            Data = comments.Select(_commentMapper.Map)!,
            PageNr = pageNr,
            PageSize = pageSize,
            PageCount = pageCount
        };
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseWithPaging<IEnumerable<Comment>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResponseWithPaging<IEnumerable<Comment>>>> GetAllReplies(
        [FromQuery] Guid parentCommentId,
        [FromQuery][Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery][Range(1, 100)] int pageSize = 25)
    {
        var userId = User.GetUserId();

        var (comments, pageCount) = await _uow.CommentService.GetAllReplies(parentCommentId, userId, pageSize, pageNr);

        return new ResponseWithPaging<IEnumerable<Comment>>
        {
            Data = comments.Select(_commentMapper.Map)!,
            PageNr = pageNr,
            PageSize = pageSize,
            PageCount = pageCount
        };
    }


    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Comment>> Add([FromBody] PostComment postComment)
    {
        var userId = User.GetUserId();

        var userInGroup = await _uow.GroupService.IsUserInGroup(userId, postComment.GroupId);
        if (!userInGroup)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Invalid user/group."
            });
        }

        var urlId = await _uow.UrlService.GetOrCreateUrlId(postComment.Url);
        
        var comment = await _uow.CommentService.Add(urlId, postComment.GroupId, userId, postComment.Text, User.GetUsername());
        await _uow.SaveChangesAsync();

        return _commentMapper.Map(comment)!;
    }

    
    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(Comment), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Comment>> AddReply([FromBody] PostReply postComment)
    {
        var userId = User.GetUserId();

        var comment = await _uow.CommentService.AddReply(postComment.ParentCommentId, postComment.ReplyToCommentId, userId, postComment.Text, User.GetUsername());
        await _uow.SaveChangesAsync();

        return _commentMapper.Map(comment)!;
    }
}