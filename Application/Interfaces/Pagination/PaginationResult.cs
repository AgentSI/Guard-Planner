namespace Application.Interfaces.Pagination
{
    public class PaginationResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
