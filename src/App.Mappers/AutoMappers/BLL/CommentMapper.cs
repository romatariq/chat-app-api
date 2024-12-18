using AutoMapper;
using Base.DAL;
using Dal = App.DTO.Private.DAL;
using Bll = App.DTO.Private.BLL;

namespace App.Mappers.AutoMappers.BLL;

public class CommentMapper: BaseMapper<Dal.Comment, Bll.Comment>
{
    public CommentMapper(IMapper mapper) : base(mapper)
    {
    }

}