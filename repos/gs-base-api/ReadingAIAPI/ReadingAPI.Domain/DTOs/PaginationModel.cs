namespace ReadingAPI.Domain.DTOs
{
    public class PaginationModel<T>
    {
        public PaginationModel(int total, int limitByPage, List<T> items)
        {
            Total = total;
            TotalPages = total == 0 ? 0 : (int)Math.Ceiling(total / (double)limitByPage);
            Items = items;
        }

        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; }
    }
}
