using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using App.DTO.Common;
using AutoMappers = App.Mappers.AutoMappers;
using Base.BLL;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;
using App.Helpers;

namespace App.BLL.Services;

public class CommentService: BaseService<Dal.Comment, Bll.Comment, ICommentRepository>, ICommentService
{

    public CommentService(IAppUOW uow, AutoMapper.IMapper mapper)
        : base(uow.CommentRepository, new AutoMappers.BLL.CommentMapper(mapper))
    {
    }

    public async Task<(IEnumerable<Bll.Comment> comments, int totalPageCount)> GetAll(Guid? userId, string encodedUrl, ESort sort, int pageNr, int pageSize)
    {
        var urlParts = UrlHelpers.ParseEncodedUrl(encodedUrl);
        var (comments, pageCount) = await Repository.GetAll(userId, urlParts.domain, urlParts.path, urlParts.parameters, sort, pageNr, pageSize);
        var bllComments = comments.Select(Mapper.Map);
        return (bllComments, pageCount)!;
    }

    public async Task<(IEnumerable<Bll.Comment> comments, int totalPageCount)> GetAllReplies(Guid parentCommentId, Guid? userId, ESort sort, int pageSize, int pageNr)
    {
        var (comments, pageCount) = await Repository.GetAllReplies(parentCommentId, userId, sort, pageSize, pageNr);
        var bllComments = comments.Select(Mapper.Map);
        return (bllComments, pageCount)!;
    }

    public async Task<Bll.Comment> Add(Guid urlId, Guid userId, string text)
    {
        var comment = await Repository.Add(urlId, userId, text);
        return Mapper.Map(comment)!;
    }

    public async Task<Bll.Comment> AddReply(Guid parentCommentId, Guid replyToCommentId, Guid userId, string text)
    {
        var comment = await Repository.AddReply(parentCommentId, replyToCommentId, userId, text);
        return Mapper.Map(comment)!;
    }
}