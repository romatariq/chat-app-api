using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using App.DTO.Public.v1;
using AutoMappers = App.Mappers.AutoMappers;
using Base.BLL;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;

namespace App.BLL.Services;

public class CommentService: BaseService<Dal.Comment, Bll.Comment, ICommentRepository>, ICommentService
{
    protected IAppUOW Uow;

    public CommentService(IAppUOW uow, AutoMapper.IMapper mapper)
        : base(uow.CommentRepository, new AutoMappers.BLL.CommentMapper(mapper))
    {
        Uow = uow;
    }

    public async Task<(IEnumerable<Bll.Comment> comments, int totalPageCount)> GetAll(Guid groupId, Guid userId, string domain, string? path, string? parameters, ESort sort, int pageNr,
        int pageSize)
    {
        var (comments, pageCount) = await Uow.CommentRepository.GetAll(groupId, userId, domain, path, parameters, sort, pageNr, pageSize);
        var bllComments = comments.Select(Mapper.Map);
        return (bllComments, pageCount)!;
    }
}