using Base.Contracts.BLL;
using Base.Contracts.DAL;

public abstract class BaseBLL<TUOW> : IBaseBLL
    where TUOW : IBaseUOW
{
    protected readonly TUOW Uow;

    protected BaseBLL(TUOW uow)
    {
        Uow = uow;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}