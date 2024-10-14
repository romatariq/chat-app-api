using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using AutoMappers = App.Mappers.AutoMappers;
using Base.BLL;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;
using App.Domain.Exceptions;

namespace App.BLL.Services;

public class CommentReactionService: BaseService<Dal.CommentReaction, Bll.CommentReaction, ICommentReactionRepository>, ICommentReactionService
{

    public CommentReactionService(IAppUOW uow, AutoMapper.IMapper mapper)
        : base(uow.CommentReactionRepository, new AutoMappers.BLL.CommentReactionMapper(mapper))
    {
    }

    public async Task<Bll.CommentReaction> Add(Bll.CommentReaction reaction)
    {
        var existingReaction = await Repository.Get(reaction.CommentId, reaction.UserId);
        if (existingReaction != null)
        {
            throw new CustomUserBadInputException("Cannot add a reaction because one already exists.");
        }

        var addedReaction = await Repository.Add(Mapper.Map(reaction)!);
        return Mapper.Map(addedReaction)!;
    }

    public async Task<Bll.CommentReaction> Update(Bll.CommentReaction reaction)
    {
        var updatedReaction = await Repository.Update(Mapper.Map(reaction)!);
        return Mapper.Map(updatedReaction)!;
    }

    public async Task Delete(Guid commentId, Guid userId)
    {
        await Repository.Delete(commentId, userId);
    }
}