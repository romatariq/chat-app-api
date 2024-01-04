using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using App.DAL.EF;
using App.Domain;
using App.Public.DTO.v1;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Comment = App.Public.DTO.v1.Comment;

namespace WebApp.ApiControllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/url/{url}/group/{groupId:guid}")]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _ctx;

    public CommentsController(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ResponseWithPaging<IEnumerable<Comment>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ResponseWithPaging<IEnumerable<Comment>>>> GetAll(
        [FromRoute] string url,
        [FromRoute] Guid groupId,
        [FromQuery] [Range(1, int.MaxValue)] int pageNr = 1,
        [FromQuery] [Range(1, 100)] int pageSize = 25)
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

        var commentsQuery = _ctx.Comments
            .Include(c => c.Url)
            .ThenInclude(u => u!.WebDomain)
            .Include(c => c.User)
            .Include(c => c.CommentReactions)
            .Where(c =>
                c.GroupId == groupId &&
                c.Url!.WebDomain!.Name == domain &&
                c.Url.Path == path &&
                c.Url.Params == parameters)
            .Select(c => new Comment
            {
                Id = c.Id,
                CreatedAtUtc = c.CreatedAtUtc,
                Text = c.Text,
                Username = c.User!.UserName!,
                Likes = c.CommentReactions!
                    .Count(cr =>
                        cr.ReactionType == ECommentReactionType.Like),
                Dislikes = c.CommentReactions!
                    .Count(cr =>
                        cr.ReactionType == ECommentReactionType.Dislike),
                HasUserLiked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Like &&
                        cr.UserId == userId),
                HasUserDisliked = c.CommentReactions!
                    .Any(cr =>
                        cr.ReactionType == ECommentReactionType.Dislike &&
                        cr.UserId == userId)
            })
            .AsQueryable();

        var totalCommentsCount = await commentsQuery.CountAsync();
        return new ResponseWithPaging<IEnumerable<Comment>>
        {
            Data = await commentsQuery
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(),
            PageNr = pageNr,
            PageSize = pageSize,
            PageCount = Math.Max(1, totalCommentsCount / pageSize + (totalCommentsCount % pageSize == 0 ? 0 : 1))
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

        var domainId = await _ctx.WebDomains
            .Where(d => d.Name == domain)
            .Select(d => (Guid?) d.Id)
            .SingleOrDefaultAsync();
        
        domainId ??= (await _ctx.WebDomains
                .AddAsync(new WebDomain()
                {
                    Name = domain
                })).Entity.Id;
        
        var urlId = await _ctx.Urls
            .Where(u => 
                u.WebDomainId == domainId &&
                u.Path == path &&
                u.Params == parameters)
            .Select(u => (Guid?) u.Id)
            .SingleOrDefaultAsync();
        
        urlId ??= (await _ctx.Urls
                .AddAsync(new Url()
                {
                    WebDomainId = domainId.Value,
                    Path = path,
                    Params = parameters
                })).Entity.Id;

        var comment = new App.Domain.Comment()
        {
            Text = postComment.Text,
            GroupId = groupId,
            UserId = userId,
            UrlId = urlId.Value
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