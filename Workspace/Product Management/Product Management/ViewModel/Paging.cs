namespace Product_Management.ViewModel
{
    public class Paging
    {
        public int PageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        public int PageCount
        {
            get
            {
                return TotalItem / PageSize + (TotalItem % PageSize > 0 ? 1 : 0);
            }
        }
        public int TotalItem { get; set; }
    }
}
