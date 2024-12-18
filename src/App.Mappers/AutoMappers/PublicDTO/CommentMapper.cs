using AutoMapper;
using Base.DAL;
using Bll = App.DTO.Private.BLL;
using PublicV1 = App.DTO.Public.v1;

namespace App.Mappers.AutoMappers.PublicDTO;

public class CommentMapper: BaseMapper<Bll.Comment, PublicV1.Comment>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }

}