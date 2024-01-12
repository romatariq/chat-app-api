using AutoMapper;
using Base.DAL;
using Bll = App.DTO.Private.BLL;
using PublicV1 = App.DTO.Public.v1;

namespace App.Mappers.AutoMappers.PublicDTO;

public class GroupMapper: BaseMapper<Bll.Group, PublicV1.Group>
{
    public GroupMapper(IMapper mapper) : base(mapper)
    {
    }

}