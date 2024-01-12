using AutoMapper;
using Base.DAL;
using Dal = App.DTO.Private.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Mappers.AutoMappers.BLL;

public class GroupMapper: BaseMapper<Dal.Group, Bll.Group>
{
    public GroupMapper(IMapper mapper) : base(mapper)
    {
    }

}