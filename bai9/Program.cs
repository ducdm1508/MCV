using bai9.Models;
using Microsoft.EntityFrameworkCore;
namespace bai9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Products}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
//Code First là một phương pháp phát triển phần mềm trong đó developer 
//    định nghĩa mô hình dữ liệu của ứng dụng bằng các lớp code (ví dụ: C#) trước, 
//    sau đó ORM (Object-Relational Mapper) (như Entity Framework) sẽ tự động tạo hoặc
//    cập nhật cơ sở dữ liệu tương ứng với các lớp code đó

//Database First
//Định nghĩa: Bắt đầu từ một cơ sở dữ liệu đã tồn tại. 
//Ưu điểm:
//Nhanh chóng: Là phương pháp nhanh hơn và dễ dàng hơn cho người mới bắt đầu. 
//Phù hợp với hệ thống cũ: Thường được ưu tiên cho các cơ sở dữ liệu hiện có hoặc legacy. 
//Nhược điểm:
//Ít kiểm soát và linh hoạt hơn so với Code First. 
//Khi nào sử dụng: Khi cơ sở dữ liệu là yếu tố ưu tiên và đã tồn tại trước khi phát triển ứng dụng. 