using AutoMapper;
using Base.DAL;
using Dal = App.DTO.Private.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Mappers.AutoMappers.BLL;

public class CommentReactionMapper: BaseMapper<Dal.CommentReaction, Bll.CommentReaction>
{
    public CommentReactionMapper(IMapper mapper) : base(mapper)
    {
    }

}