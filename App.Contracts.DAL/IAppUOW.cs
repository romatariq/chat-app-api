using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IBaseUOW
{
    ICommentRepository CommentRepository { get; }
    IUrlRepository UrlRepository { get; }
}