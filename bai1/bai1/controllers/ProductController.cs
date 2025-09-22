using bai1.models;
using Microsoft.AspNetCore.Mvc;

namespace bai1.controllers
{
    public class ProductController : Controller
    {
        private static List<Product> products = new List<Product>
        {
            new Product{ Id=1, Name="Product A", Price=100 },
            new Product{ Id=2, Name="Product B", Price=200 },
            new Product{ Id=3, Name="Product C", Price=300 }
        };
        public IActionResult Product()
        {
            return View("Product", products);
        }
        public IActionResult Details(int id)
        { 
            var product = products.Find(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
