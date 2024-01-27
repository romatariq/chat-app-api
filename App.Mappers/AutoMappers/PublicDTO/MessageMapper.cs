using AutoMapper;
using Base.DAL;
using Bll = App.DTO.Private.BLL;
using PublicV1 = App.DTO.Public.v1;

namespace App.Mappers.AutoMappers.PublicDTO;

public class MessageMapper: BaseMapper<Bll.Message, PublicV1.Message>
{
    public MessageMapper(IMapper mapper) : base(mapper)
    {
    }

}