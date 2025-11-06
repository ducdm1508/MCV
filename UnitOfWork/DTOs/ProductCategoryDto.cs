namespace UnitOfWork.DTOs
{
    public class ProductCategoryDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
    }
}
