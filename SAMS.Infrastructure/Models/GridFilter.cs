namespace SAMS.Infrastructure.Models
{
    public class GridFilter
    {
        public string SortBy { get; set; }

        public bool IsSortAscending { get; set; }

        public int PageFirstIndex { get; set; }

        public byte PageSize { get; set; }
    }
}
