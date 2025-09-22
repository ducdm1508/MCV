using bai3.Models;
using Microsoft.AspNetCore.Mvc;

namespace bai3.Controllers
{
    public class VietNamController : Controller
    {

        public static List<City> Cities = new List<City>()
        {
            new City { id = 1, name = "Thành phố Hà Nội" },
            new City { id = 2, name = "Thành phố Huế" },
            new City { id = 3, name = "Tỉnh Lai Châu" },
            new City { id = 4, name = "Tỉnh Điện Biên" },
            new City { id = 5, name = "Tỉnh Sơn La" },
            new City { id = 6, name = "Tỉnh Lạng Sơn" },
            new City { id = 7, name = "Tỉnh Quảng Ninh" },
            new City { id = 8, name = "Tỉnh Thanh Hóa" },
            new City { id = 9, name = "Tỉnh Nghệ An" },
            new City { id = 10, name = "Tỉnh Hà Tĩnh" },
            new City { id = 11, name = "Tỉnh Cao Bằng" },

            new City { id = 12, name = "Tỉnh Tuyên Quang" },
            new City { id = 13, name = "Tỉnh Lào Cai" },
            new City { id = 14, name = "Tỉnh Thái Nguyên" },
            new City { id = 15, name = "Tỉnh Phú Thọ" },
            new City { id = 16, name = "Tỉnh Bắc Ninh" },
            new City { id = 17, name = "Tỉnh Hưng Yên" },
            new City { id = 18, name = "Thành phố Hải Phòng" },
            new City { id = 19, name = "Tỉnh Ninh Bình" },
            new City { id = 20, name = "Tỉnh Quảng Trị" },
            new City { id = 21, name = "Thành phố Đà Nẵng" },
            new City { id = 22, name = "Tỉnh Quảng Ngãi" },
            new City { id = 23, name = "Tỉnh Gia Lai" },
            new City { id = 24, name = "Tỉnh Khánh Hòa" },
            new City { id = 25, name = "Tỉnh Lâm Đồng" },
            new City { id = 26, name = "Tỉnh Đắk Lắk" },
            new City { id = 27, name = "Thành phố Hồ Chí Minh" },
            new City { id = 28, name = "Tỉnh Đồng Nai" },
            new City { id = 29, name = "Tỉnh Tây Ninh" },
            new City { id = 30, name = "Thành phố Cần Thơ" },
            new City { id = 31, name = "Tỉnh Vĩnh Long" },
            new City { id = 32, name = "Tỉnh Đồng Tháp" },
            new City { id = 33, name = "Tỉnh Cà Mau" },
            new City { id = 34, name = "Tỉnh An Giang" }
        };
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ThanhPho()
        {
            return View(Cities);
        }
    }
}
