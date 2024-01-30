using AutoMapper;
using Base.DAL;
using Bll = App.DTO.Private.BLL;
using PublicV1 = App.DTO.Public.v1;

namespace App.Mappers.AutoMappers.PublicDTO;

public class CommentReactionMapper: BaseMapper<Bll.CommentReaction, PublicV1.CommentReaction>
{
    public CommentReactionMapper(IMapper mapper) : base(mapper)
    {
    }

}