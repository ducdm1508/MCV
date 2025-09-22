using bai4.Models;
using Microsoft.AspNetCore.Mvc;

namespace bai4.Controllers
{
    public class MovieController : Controller
    {
        private static List<Movie> movies = new List<Movie>
        {
            new Movie {
                Id = 1,
                Title = "Doraemon: Nobita's Dinosaur (2006)",
                Description = "Nobita tình cờ tìm thấy trứng khủng long và cuộc phiêu lưu bắt đầu.",
                Price = 0,
                Director = "Ayumu Watanabe",
                Trailer = "https://www.youtube.com/embed/m1ScW_PgUzU?si=vhhBOyOp4izNd25b",
                WatchLink = "/videos/1/master.m3u8",
                Poster = "/img/dino.jpg"
            },
            new Movie {
                Id = 2,
                Title = "Doraemon: Nobita and the Steel Troops (2011)",
                Description = "Nobita và Doraemon chế tạo người máy khổng lồ và phải đối đầu với quân xâm lược.",
                Price = 0,
                Director = "Yukiyo Teramoto",
                Trailer = "https://www.youtube.com/embed/4KzYsl6ucL8?si=g1B_OhHzvLUNRc3y",
                WatchLink = "/videos/2/master.m3u8",
                Poster = "/img/steeltroop.jpg"
            },
            new Movie {
                Id = 3,
                Title = "Doraemon Movie - Nobita and the Adventures in the World of Paintings",
                Description = "Nobita lạc vào thế giới ma thuật cùng bạn bè và Doraemon.",
                Price = 0,
                Director = "Yoshihiro Osugi",
                Trailer = "https://www.youtube.com/embed/x97LAn84wkY?si=ol2bgJ3SNrP5Lx3v",
                WatchLink = "/videos/3/master.m3u8",
                Poster = "/img/magic.jpg"
            },
            new Movie {
                Id = 4,
                Title = "Doraemon: Nobita's Secret Gadget Museum (2013)",
                Description = "Doraemon bị mất chuông và nhóm bạn bước vào hành trình tìm lại.",
                Price = 0,
                Director = "Yukiyo Teramoto",
                Trailer = "https://www.youtube.com/embed/8cWZMh-UPhI?si=1ozbTmDaiVPsn_GG",
                WatchLink = "/videos/4/master.m3u8",
                Poster = "/img/Dosecret.jpg"
            },
            new Movie {
                Id = 5,
                Title = "Doraemon: Nobita's Chronicle of the Moon Exploration (2019)",
                Description = "Nobita phát hiện ra thành phố bí mật trên Mặt Trăng.",
                Price = 0,
                Director = "Shinnosuke Yakuwa",
                Trailer = "https://www.youtube.com/embed/1fYvUTlmvDs?si=YTy5sNsKarLZ8CA2",
                WatchLink = "/videos/5/master.m3u8",
                Poster = "/img/moon.jpg"
            },
        };

        public IActionResult Index()
        {
            return View(movies);
        }

        public IActionResult Details(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();
            return View(movie);
        }

        public IActionResult Watch(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null) return NotFound();

            var related = movies.Where(m => m.Id != id).Take(6).ToList();
            ViewBag.Related = related;

            return View(movie);
        }
    }
}
