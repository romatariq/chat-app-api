using App.Contracts.DAL.IRepositories;
using App.Domain.Exceptions;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Dal = App.DTO.Private.DAL;
using Domain = App.Domain;

namespace App.DAL.EF.Repositories;

public class CommentReactionRepository: EfBaseRepository<Domain.CommentReaction, AppDbContext>, ICommentReactionRepository
{
    public CommentReactionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Dal.CommentReaction> Add(Dal.CommentReaction reaction)
    {
        var dbEntityEntry = (await DbSet.AddAsync(new Domain.CommentReaction()
        {
            ReactionType = reaction.ReactionType,
            CommentId = reaction.CommentId,
            UserId = reaction.UserId
        })).Entity;

        return new Dal.CommentReaction()
        {
            Id = dbEntityEntry.Id,
            ReactionType = dbEntityEntry.ReactionType,
            CommentId = dbEntityEntry.CommentId,
            UserId = dbEntityEntry.UserId
        };
    }

    public async Task<Dal.CommentReaction> Update(Dal.CommentReaction reaction)
    {
        var dbReaction = await Get(reaction.CommentId, reaction.UserId);
        if (dbReaction == null)
        {
            throw new CustomUserBadInputException("Cannot update reaction that does not exist.");
        }
        
        dbReaction.ReactionType = reaction.ReactionType;

        return new Dal.CommentReaction()
        {
            Id = dbReaction.Id,
            ReactionType = dbReaction.ReactionType,
            CommentId = dbReaction.CommentId,
            UserId = dbReaction.UserId
        };
    }

    public async Task Delete(Dal.CommentReaction reaction)
    {
        var reactionFromDb = await Get(reaction.CommentId, reaction.UserId);
        if (reactionFromDb == null)
        {
            throw new CustomUserBadInputException("Cannot delete reaction that doesn't exist");
        }
        DbSet.Remove(reactionFromDb);
    }

    public async Task<Domain.CommentReaction?> Get(Guid commentId, Guid userId)
    {
        return await DbSet
            .Where(cr => 
                cr.CommentId == commentId && 
                cr.UserId == userId)
            .SingleOrDefaultAsync();
    }
}