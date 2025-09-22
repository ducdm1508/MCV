namespace bai2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            var app = builder.Build();

            app.UseRouting();

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller=Product}/{action=index}/{id?}"
                );

            app.Run();
        }
    }
}
