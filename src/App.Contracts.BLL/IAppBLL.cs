using App.Contracts.BLL.IServices;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL: IBaseBLL
{
    ICommentService CommentService { get; }
    IUrlService UrlService { get; }
    IMessageService MessageService { get; }
    ICommentReactionService CommentReactionService { get; }
    IDomainReportService DomainReportService { get; }
}