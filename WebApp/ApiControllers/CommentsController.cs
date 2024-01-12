using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain;
using App.DTO.Public.v1;
using App.Mappers.AutoMappers.PublicDTO;
using Asp.Versioning;
using AutoMapper;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Comment = App.DTO.Public.v1.Comment;

namespace WebApp.ApiControllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/url/{url}/group/{groupId:guid}")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _ctx;
    private readonly IAppBLL _uow;
    private readonly CommentMapper _commentMapper;

    public CommentsController(AppDbContext ctx, IAppBLL uow, IMapper autoMapper)
    {
        _ctx = ctx;
        _uow = uow;
        _commentMapper = new CommentMapper(autoMapper);
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseWithPaging<IEnumerable<Comment>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResponseWithPaging<IEnumerable<Comment>>>> GetAll(
        [FromRoute] string url,
        [FromRoute] Guid groupId,
        [FromQuery] [Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 25,
        [FromQuery]  ESort sort = ESort.Top)
    {
        var userId = User.GetUserId();

        var userInGroup = await _ctx.GroupUsers
            .AnyAsync(gu =>
                gu.UserId == userId &&
                gu.GroupId == groupId);

        if (!userInGroup)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Invalid user/group."
            });
        }

        var (domain, path, parameters) = UrlHelpers.ParseEncodedUrl(url);

        var (comments, pageCount) = await _uow.CommentService
                .GetAll(groupId, userId, domain, path, parameters, sort, pageNr, pageSize);

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
    public async Task<ActionResult<Comment>> Add(
        [FromRoute] string url,
        [FromRoute] Guid groupId,
        [FromBody] PostComment postComment)
    {
        var userId = User.GetUserId();

        var userInGroup = await _ctx.GroupUsers
            .AnyAsync(gu =>
                gu.UserId == userId &&
                gu.GroupId == groupId);

        if (!userInGroup)
        {
            return BadRequest(new ErrorResponse
            {
                Error = "Invalid user/group."
            });
        }

        var (domain, path, parameters) = UrlHelpers.ParseEncodedUrl(url);

        var domainId = await _uow.UrlService.GetOrCreateDomainId(domain);
        var urlId = await _uow.UrlService.GetOrCreateUrlId(domainId, path, parameters);

        var comment = new App.Domain.Comment()
        {
            Text = postComment.Text,
            GroupId = groupId,
            UserId = userId,
            UrlId = urlId
        };

        await _ctx.Comments.AddAsync(comment);
        await _ctx.SaveChangesAsync();

        return new Comment()
        {
            Text = comment.Text,
            Username = User.GetUsername(),
            CreatedAtUtc = DateTime.UtcNow,
            Id = comment.Id
        };
    }
}