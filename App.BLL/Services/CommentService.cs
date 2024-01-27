using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using App.DTO.Private.Shared;
using AutoMappers = App.Mappers.AutoMappers;
using Base.BLL;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;

namespace App.BLL.Services;

public class CommentService: BaseService<Dal.Comment, Bll.Comment, ICommentRepository>, ICommentService
{

    public CommentService(IAppUOW uow, AutoMapper.IMapper mapper)
        : base(uow.CommentRepository, new AutoMappers.BLL.CommentMapper(mapper))
    {
    }

    public async Task<(IEnumerable<Bll.Comment> comments, int totalPageCount)> GetAll(GetAllCommentsParameters parameters)
    {
        var (comments, pageCount) = await Repository.GetAll(parameters);
        var bllComments = comments.Select(Mapper.Map);
        return (bllComments, pageCount)!;
    }

    public async Task<Bll.Comment> Add(Guid urlId, Guid groupId, Guid userId, string text, string username)
    {
        var comment = await Repository.Add(urlId, groupId, userId, text, username);
        return Mapper.Map(comment)!;
    }
}