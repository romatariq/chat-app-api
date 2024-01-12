using App.Contracts.DAL.IRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Dal = App.DTO.Private.DAL;

namespace App.DAL.EF.Repositories;

public class GroupRepository: EfBaseRepository<Group, AppDbContext>, IGroupRepository
{
    public GroupRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Dal.Group>> GetAll(Guid userId)
    {
        return await DbContext.GroupUsers
            .Include(gu => gu.Group)
            .Where(gu => gu.UserId == userId)
            .Select(gu => new Dal.Group
            {
                Id = gu.GroupId,
                Name = gu.Group!.Name,
                Type = gu.Group.GroupType,
                IsOwner = gu.IsOwner
            })
            .ToListAsync();
    }
}