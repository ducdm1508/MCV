namespace shopapp.Models
{
    public class CategoryWithProducts
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
