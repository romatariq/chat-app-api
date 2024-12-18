using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using AutoMappers = App.Mappers.AutoMappers;
using Base.BLL;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;

namespace App.BLL.Services;

public class MessageService: BaseService<Dal.Message, Bll.Message, IMessageRepository>, IMessageService
{

    public MessageService(IAppUOW uow, AutoMapper.IMapper mapper)
        : base(uow.MessageRepository, new AutoMappers.BLL.MessageMapper(mapper))
    {
    }

    public async Task<Bll.Message> Add(string message, Guid urlId, Guid userId, string username)
    {
        var addedMessage = await Repository.Add(message, urlId, userId, username);
        return Mapper.Map(addedMessage)!;
    }

    public async Task<IEnumerable<Bll.Message>> GetPreviousMessages(Guid urlId)
    {
        var messages = await Repository.GetPreviousMessages(urlId);
        return messages.Select(Mapper.Map)!;
    }
}