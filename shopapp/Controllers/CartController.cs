using Microsoft.AspNetCore.Mvc;

namespace shopapp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
