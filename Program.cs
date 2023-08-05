// ========= BÀI 58: (ASP.NET Razor 09) Tích hợp Entity Framework vào ASP.NET, làm việc với SQL Server ============
using ASP.NET_RazorPage_P8.Models;
using Microsoft.AspNetCore.Hosting;

namespace ASP.NET_RazorPage_P8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

/*
    ---------- Cài đặt các package tích hợp Entity Framework vào ASP.NET ------------
    ---------- Khai báo tạo ra Model biểu diễn cấu trúc bảng dữ liệu SQL ------------
    ---------- Khai báo tạo DbContext, cấu trúc CSDL ------------
    ---------- Cấu hình chuỗi kết nối, đăng ký DbContext vào DI của ứng dụng ------------
    không phải tạo ra đối tượng DbContext nữa mà ta phải đăng ký nó như là một dịnh vụ của app
    
    B1: vào appsettings.jon thêm thông tin sau:
        "ConnectionStrings": {
            "TenDbContext": "Data Source=DESKTOP-F148RFH\\KHANG; Database=razorwebdb ; User ID=sa; Password=sa123456"
        }
    
    B2: tiến hành đăng ký dịch vụ:
        services.AddDbContext<TenDbContext>(options =>
            {
                string connectString = Configuration.GetConnectionString("TenDbContext");
                options.UseSqlServer(connectString);
            });

    ---------- Thực hiện cập nhật DbContext lên SQL Server với Migration ------------
    dotnet ef migrations add tenPhienBan (Status: Pending -> chưa update lên DB gốc)
    dotnet ef migrations remove
    dotnet ef migrations list
    dontet ef database update [phien-ban-muon-rollback]
    dotnet ef database drop -f

    --------- Seed Database với thư viện Bogus -----------

    --------- Inject DbContext và sử dụng để tương tác với CSDL SQL Server ----------
    
 
*/