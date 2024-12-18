using System.ComponentModel.DataAnnotations;
using App.Contracts.BLL;
using App.DTO.Common;
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
    public async Task<ResponseWithPaging<Comment>> GetAll(
        [FromQuery] string url,
        [FromQuery] [Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 25,
        [FromQuery] ESort sort = ESort.Top)
    {
        Guid? userId = User.IsAuthenticated() ? User.GetUserId() : null;

        var (comments, pageCount) = await _uow.CommentService
            .GetAll(userId, url, sort, pageNr, pageSize);

        return new ResponseWithPaging<Comment>
        {
            Data = comments.Select(_commentMapper.Map)!,
            PageNr = pageNr,
            PageSize = pageSize,
            PageCount = pageCount
        };
    }

    [HttpGet("replies")]
    public async Task<ResponseWithPaging<Comment>> GetAllReplies(
        [FromQuery] [Required] Guid parentCommentId,
        [FromQuery] [Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 10,
        [FromQuery] ESort sort = ESort.Old)
    {
        Guid? userId = User.IsAuthenticated() ? User.GetUserId() : null;

        var (comments, pageCount) = await _uow.CommentService.GetAllReplies(parentCommentId, userId, sort, pageSize, pageNr);

        return new ResponseWithPaging<Comment>
        {
            Data = comments.Select(_commentMapper.Map)!,
            PageNr = pageNr,
            PageSize = pageSize,
            PageCount = pageCount
        };
    }


    [HttpPost]
    [Authorize]
    public async Task<Comment> Add([FromBody] PostComment postComment)
    {
        var userId = User.GetUserId();

        var urlId = await _uow.UrlService.GetOrCreateUrlId(postComment.Url);
        
        var comment = await _uow.CommentService.Add(urlId, userId, postComment.Text);
        await _uow.SaveChangesAsync();
        comment.Username = User.GetUsername();

        return _commentMapper.Map(comment)!;
    }

    
    [HttpPost("replies")]
    [Authorize]
    public async Task<Comment> AddReply([FromBody] PostReply postComment)
    {
        var userId = User.GetUserId();

        var comment = await _uow.CommentService.AddReply(postComment.ParentCommentId, postComment.ReplyToCommentId, userId, postComment.Text);
        await _uow.SaveChangesAsync();
        comment.Username = User.GetUsername();

        return _commentMapper.Map(comment)!;
    }
}