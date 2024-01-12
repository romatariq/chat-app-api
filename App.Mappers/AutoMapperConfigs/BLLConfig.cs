using AutoMapper;
using Bll = App.DTO.Private.BLL;
using Dal = App.DTO.Private.DAL;

namespace App.Mappers.AutoMapperConfigs;

public class BLLConfig: Profile
{
    public BLLConfig()
    {
        CreateMap<Dal.Comment, Bll.Comment>().ReverseMap();
        CreateMap<Dal.Group, Bll.Group>().ReverseMap();
    }
}
