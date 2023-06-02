namespace Talabat.Core.Specification
{
    public class ProductsSpecParams
    {
        private const int MaxPageSize = 10;
        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }

        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }

        private string? search;

        public string? Search
        {
            get { return search; }
            set
            {
                search = value.ToLower();
            }
        }

    }


}
