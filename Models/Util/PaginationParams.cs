namespace fw_shop_api.Models.Util
{
    public class PaginationParams
    {
        private const int MaxPageSize = 20;
        private int _pageSize = 5;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get {return _pageSize;} set {_pageSize = value > MaxPageSize ? MaxPageSize : value;} }
    }
}