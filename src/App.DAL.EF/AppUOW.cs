using App.Contracts.DAL;
using App.Contracts.DAL.IRepositories;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : EfBaseUOW<AppDbContext>, IAppUOW
{

    public AppUOW(AppDbContext context) : base(context)
    {
    }

    private ICommentRepository? _commentRepository;
    private IUrlRepository? _urlRepository;
    private IMessageRepository? _messageRepository;
    private ICommentReactionRepository? _commentReactionRepository;
    private IDomainReportRepository? _domainReportRepository;
    
 
    public ICommentRepository CommentRepository =>
        _commentRepository ??= new CommentRepository(DbContext);
    
    public IUrlRepository UrlRepository =>
        _urlRepository ??= new UrlRepository(DbContext);

    public IMessageRepository MessageRepository =>
        _messageRepository ??= new MessageRepository(DbContext);

    public ICommentReactionRepository CommentReactionRepository =>
        _commentReactionRepository ??= new CommentReactionRepository(DbContext);

    public IDomainReportRepository DomainReportRepository =>
        _domainReportRepository ??= new DomainReportRepository(DbContext);
}