using App.Contracts.DAL.IRepositories;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Dal = App.DTO.Private.DAL;
using Domain = App.Domain;

namespace App.DAL.EF.Repositories;

public class MessageRepository: EfBaseRepository<Domain.Message, AppDbContext>, IMessageRepository
{
    public MessageRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<Dal.Message> Add(string message, Guid urlId, Guid userId, string username)
    {
        var domainMessage = new Domain.Message()
        {
            Text = message,
            UserId = userId,
            UrlId = urlId
        };

        await DbSet.AddAsync(domainMessage);

        return new Dal.Message()
        {
            Id = domainMessage.Id,
            CreatedAtUtc = domainMessage.CreatedAtUtc,
            Text = domainMessage.Text,
            Username = username
        };
    }

    public async Task<IEnumerable<Dal.Message>> GetPreviousMessages(Guid urlId)
    {
        return await DbSet
            .Include(m => m.User)
            .Where(m => m.UrlId == urlId)
            .OrderBy(m => m.CreatedAtUtc)
            .Select(m => new Dal.Message()
            {
                Id = m.Id,
                CreatedAtUtc = m.CreatedAtUtc,
                Text = m.Text,
                Username = m.User!.UserName!
            })
            .Take(50)
            .ToListAsync();
    }
}