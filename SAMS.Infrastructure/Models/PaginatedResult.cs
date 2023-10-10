namespace SAMS.Infrastructure.Models
{
    public class GridResult<T>
    {
        public T Result { get; set; }

        public long TotalCount { get; set; }
    }
}