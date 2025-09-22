using Microsoft.AspNetCore.Mvc;

namespace bai1.controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
