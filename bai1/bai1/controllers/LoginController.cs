using bai1.models;
using Microsoft.AspNetCore.Mvc;

namespace bai1.controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login(Acc acc)
        {


            if (acc != null &&
                !string.IsNullOrWhiteSpace(acc.Username) &&
                !string.IsNullOrWhiteSpace(acc.Password))
            {
                string username = acc.Username.Trim();
                string password = acc.Password.Trim();

                if (username == "admin" && password == "123")
                {
                    ViewBag.Message = "Login thành công!";
                }
                else
                {
                    ViewBag.Message = $"Sai username hoặc password! Username='{username}', Password='{password}'";
                }
            }
            else
            {
                ViewBag.Message = "Vui lòng nhập đầy đủ thông tin!";
            }

            return View();
        }
    }
}