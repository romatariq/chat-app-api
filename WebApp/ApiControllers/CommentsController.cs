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
[Route("api/v{version:apiVersion}/[controller]/url/{url}/group/{groupId:guid}")]
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
        [FromRoute] string url,
        [FromRoute] Guid groupId,
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

        var userInGroup = await _uow.GroupService.IsUserInGroup(userId, groupId);
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
        
        var comment = await _uow.CommentService.Add(urlId, groupId, userId, postComment.Text, User.GetUsername());
        await _uow.SaveChangesAsync();

        return _commentMapper.Map(comment)!;
    }
}