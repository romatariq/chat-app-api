using AutoMapper;
using Base.DAL;
using Dal = App.DTO.Private.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Mappers.AutoMappers.BLL;

public class MessageMapper: BaseMapper<Dal.Message, Bll.Message>
{
    public MessageMapper(IMapper mapper) : base(mapper)
    {
    }

}