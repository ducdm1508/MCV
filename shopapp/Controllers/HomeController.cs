using Microsoft.AspNetCore.Mvc;
using shopapp.Models;

namespace shopapp.Controllers
{
    public class HomeController : Controller
    {
        private List<Category> categories = new List<Category>
        {
            new Category { Id = 1, Name = "Điện thoại" },
            new Category { Id = 2, Name = "Laptop" }
        };

        private List<Product> products = new List<Product>
            {
            new Product { Id = 1, Name = "Iphone 8", ImageUrl="/images/iphone8.jpg", Price=800, Description="Iphone 8 chính hãng", CategoryId=1 },
            new Product { Id = 2, Name = "Iphone 10", ImageUrl="/images/iphone10.jpg", Price=1000, Description="Iphone 10 bản quốc tế", CategoryId=1 },
            new Product { Id = 3, Name = "Samsung Galaxy Z Flip3", ImageUrl="/images/zflip3.jpg", Price=500, Description="Điện thoại Android", CategoryId=1 },
            new Product { Id = 4, Name = "Samsung Galaxy 25 ultra", ImageUrl="/images/s25.jpg", Price=700, Description="Samsung Galaxy chính hãng", CategoryId=1 },

            new Product { Id = 5, Name = "M1", ImageUrl="/images/m1.jpg", Price=1200, Description="Laptop chip M1", CategoryId=2 },
            new Product { Id = 6, Name = "M2", ImageUrl="/images/m2.jpg", Price=1400, Description="Laptop chip M2", CategoryId=2 },
            new Product { Id = 7, Name = "M3", ImageUrl="/images/m3.jpg", Price=1600, Description="Laptop chip M3", CategoryId=2 },
            new Product { Id = 8, Name = "M4", ImageUrl="/images/m4.jpg", Price=1800, Description="Laptop chip M4", CategoryId=2 }
            };
        public IActionResult Index(int? categoryId)
        {
            var categorizedProducts = categories.Select(c => new CategoryWithProducts
            {
                Category = c,
                Products = products.Where(p => p.CategoryId == c.Id).ToList()
            }).ToList();

            if (categoryId.HasValue)
            {
                categorizedProducts = categorizedProducts
                    .Where(cp => cp.Category.Id == categoryId.Value)
                    .ToList();
            }

            return View(categorizedProducts);
        }

    }
}
