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
    private IGroupRepository? _groupRepository;
    
 
    public ICommentRepository CommentRepository =>
        _commentRepository ??= new CommentRepository(DbContext);
    
    public IUrlRepository UrlRepository =>
        _urlRepository ??= new UrlRepository(DbContext);
    
    public IGroupRepository GroupRepository =>
        _groupRepository ??= new GroupRepository(DbContext);
}