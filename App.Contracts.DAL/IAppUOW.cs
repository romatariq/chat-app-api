using App.Contracts.DAL.IRepositories;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IBaseUOW
{
    ICommentRepository CommentRepository { get; }
    IUrlRepository UrlRepository { get; }
    IGroupRepository GroupRepository { get; }
    IMessageRepository MessageRepository { get; }
    ICommentReactionRepository CommentReactionRepository { get; }
}