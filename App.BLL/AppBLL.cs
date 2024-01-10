using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.IServices;
using App.Contracts.DAL;
using AutoMapper;

namespace App.BLL;

public class AppBLL : BaseBLL<IAppUOW>, IAppBLL
{
    private readonly AutoMapper.IMapper _mapper;

    public AppBLL(IAppUOW uow, IMapper mapper) : base(uow)
    {
        _mapper = mapper;
    }


    private ICommentService? _commentService;
    
    
    public ICommentService CommentService =>
        _commentService ??= new CommentService(Uow, _mapper);
}
