namespace Queries
{
    public class Paging
    {
        public Paging(int startIndex, int pageSize)
        {
            StartIndex = startIndex;
            PageSize = pageSize;
        }

        public int StartIndex { get; }
        public int PageSize { get; }
    }
}
