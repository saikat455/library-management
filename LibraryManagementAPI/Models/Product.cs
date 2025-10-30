namespace LibraryManagementAPI.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
    }
}