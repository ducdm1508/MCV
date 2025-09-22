using bai2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace bai2.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> products = new List<Product>
        {
            new Product{ id = 1, name = "product 1", price = 45000 },
            new Product{ id = 2, name = "product 2", price = 415000 },
            new Product{ id = 3, name = "product 3", price = 85000 },
        };

        public IActionResult Index()
        {
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = products.FirstOrDefault(p => p.id == id);

            if (product == null)
            {
                return NotFound(); 
            }

            return View(product);
        }
    }
}
