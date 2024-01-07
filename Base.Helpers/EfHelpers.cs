namespace Base.Helpers;

public static class EfHelpers
{
    public static int GetPageCount(int totalItemCount, int pageSize)
    {
        return Math.Max(1, totalItemCount / pageSize + (totalItemCount % pageSize == 0 ? 0 : 1));
    }
}