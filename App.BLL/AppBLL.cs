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
    private IUrlService? _urlService;
    private IMessageService? _messageService;
    private ICommentReactionService? _commentReactionService;
    private IDomainReportService? _domainReportService;
    
    
    public ICommentService CommentService =>
        _commentService ??= new CommentService(Uow, _mapper);
    
    public IUrlService UrlService =>
        _urlService ??= new UrlService(Uow);
    
    public IMessageService MessageService =>
        _messageService ??= new MessageService(Uow, _mapper);

    public ICommentReactionService CommentReactionService =>
        _commentReactionService ??= new CommentReactionService(Uow, _mapper);

    public IDomainReportService DomainReportService =>
        _domainReportService ??= new DomainDomainReportService(Uow);
}
