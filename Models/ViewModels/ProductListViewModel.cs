namespace DrinkStore.Models.ViewModels
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        public int? SelectedCategoryId { get; set; }
        public string SearchTerm { get; set; }
        public string SortOrder { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 8;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
}
