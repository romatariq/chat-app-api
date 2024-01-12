using AutoMapper;
using Bll = App.DTO.Private.BLL;
using PublicV1 = App.DTO.Public.v1;

namespace App.Mappers.AutoMapperConfigs;

public class PublicDTOConfig : Profile
{
    public PublicDTOConfig()
    {
        CreateMap<Bll.Comment, PublicV1.Comment>().ReverseMap();
        CreateMap<Bll.Group, PublicV1.Group>().ReverseMap();
    }
}