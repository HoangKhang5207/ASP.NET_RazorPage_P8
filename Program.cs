// ========= BÀI 58: (ASP.NET Razor 09) Tích hợp Entity Framework vào ASP.NET, làm việc với SQL Server ============
// ========= BÀI 59: (ASP.NET Razor 10) Tạo các trang CRUD làm việc với DbContext EF, SQL Server trong ASP.NET ==============
// ========= BÀI 60: (ASP.NET Razor 11) Sử dụng Identity xác thực người dùng, chức năng đăng nhập tài khoản ============
// ========= BÀI 61: (ASP.NET Razor 12) Tùy biến trang đăng ký tài khoản và xác thực email ============
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
    
    ============================================================================================
    CÁC THAO TÁC CRUD: CREATE - READ/RETRIEVE - UPDATE - DELETE
    --------- Phát sinh các trang CRUD cho Model làm việc với SQL Server ----------
    B1. Nạp các package này:
        dotnet tool uninstall --global dotnet-aspnet-codegenerator
        dotnet tool install --global dotnet-aspnet-codegenerator
        dotnet tool uninstall --global dotnet-ef
        dotnet tool install --global dotnet-ef
        dotnet add package Microsoft.EntityFrameworkCore.Design
        dotnet add package Microsoft.EntityFrameworkCore.SQLite
        dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
        dotnet add package Microsoft.EntityFrameworkCore.SqlServer
        dotnet add package Microsoft.EntityFrameworkCore.Tools

    B2. dùng câu lệnh sau:
        dotnet aspnet-codegenerator razorpage -m "TenModel" -dc "TenDbContext" -udl -outDir "ThuMucChuaCRUDFile" --referenceScriptLibraries

    ----------- Tùy biến trang Index hiện thị danh sách bài viết ------------
    ----------- Xây dựng tính năng tìm kiếm bài viết ----------
    ----------- Chức năng Create tạo bài viết mới ----------
    ----------- Chức năng Edit cập nhật bài viết -----------
    ----------- Chức năng xem bài viết chi tiết Detail ----------
    ----------- Chức năng xóa dữ liệu Delete ------------
    ----------- Xây dựng chức năng paging -----------
    ----------- Lưu dự án với Git -----------

    ============================================================================================
    --------- Giới thiệu Identity, cài đặt các package Identity vào ứng dụng ASP.NET --------
    ******* dùng các command sau:
        dotnet add package System.Data.SqlClient
        dotnet add package Microsoft.EntityFrameworkCore
        dotnet add package Microsoft.EntityFrameworkCore.SqlServer
        dotnet add package Microsoft.EntityFrameworkCore.Design
        dotnet add package Microsoft.Extensions.DependencyInjection
        dotnet add package Microsoft.Extensions.Logging.Console

        dotnet add package Microsoft.AspNetCore.Identity
        dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
        dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
        dotnet add package Microsoft.AspNetCore.Identity.UI
        dotnet add package Microsoft.AspNetCore.Authentication
        dotnet add package Microsoft.AspNetCore.Http.Abstractions
        dotnet add package Microsoft.AspNetCore.Authentication.Cookies
        dotnet add package Microsoft.AspNetCore.Authentication.Facebook
        dotnet add package Microsoft.AspNetCore.Authentication.Google
        dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
        dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
        dotnet add package Microsoft.AspNetCore.Authentication.oAuth
        dotnet add package Microsoft.AspNetCore.Authentication.OpenIDConnect
        dotnet add package Microsoft.AspNetCore.Authentication.Twitter

    **** Identity:
        - Authentication: Xác định danh tính -> Login, Logout ...
        - Authorization: Xác thực quyền truy cập
        - Quản lí user: Sign Up, User, Role...
    **** Cấu hình quan trọng đầu tiên là các Middleware ở trong file Startup.cs
    * Ở phương thức Configure, sau app.UseRouting() bắt buộc phải có 2 middleware sau
        - UseAuthentication() và UseAuthorization()

    --------- Cấu trúc bảng CSDL của Identity ------------
    ***** Có các Model được xây dựng sẵn trong Identity tương ứng với cái Table trong CSDL gốc
    * Ví dụ như Model IdentityUser đại diện cho Table Users -> ta hoàn toàn có thể bổ sung các
    * thuộc tính thêm cho Table Users ngoài các thuộc tính có sẵn bằng cách xây dựng một lớp
    * kế thừa từ class IdentityUser
    * 
    * Ví dụ như Model IdentityDbContext được implement từ DbContext mà tương ứng với một CSDL thực
    * gồm các Table có sẵn trong Identity, ta có thể xây dựng một Context khác kế thừa từ IdentityDbContext

    --------- Đăng ký các dịch vụ của Identity -----------

    --------- Thực hiện ef migration cập nhật CSDL Identity ---------
    **** Xử lí bỏ tiền tố AspNet_ ở các Table của IdentityDbContext

    --------- Một số thiết lập cho các dịch vụ Identity -------------

    --------- Sử dụng giao diện mặc định (UI) của Identity ----------

    --------- Xây dụng partial  _LoginPartial.cshtml, menu thông tin đăng nhập ---------

    --------- Đăng ký, đăng nhập, trong Identity ----------

    --------- Tạo dịch vụ IMailSender sử dụng bởi Identity ----------
    * Thêm 2 package: MailKit và MimeKit
    * Sau đó tạo thư mục Services
    * Thêm vào appsettings.json:
        "MailSettings": {
            "Mail": "gmail của bạn",
            "DisplayName": "Tên Hiện Thị (ví dụ XUANTHULAB)",
            "Password": "passowrd ở đây",
            "Host": "smtp.gmail.com",
            "Port": 587
        }
    * Thiết lập lớp SendMailService là đăng ký dịch vụ vào app để Identity sử dụng

    --------- (scaffold) Phát sinh code các trang Razor của Identity -----------
    Phát sinh code của các trang Login, Logout, Register,... của Identity để có thể tùy biến
    các View, PageModel tùy theo sở thích của chúng ta
    ***** Sử dụng câu lệnh sau: dotnet aspnet-codegenerator identity -dc ASP.NET_RazorPage_P8.Models.MyBlogContext 
    
    ==============================================================================================================================
    -------------- Giới thiệu và tùy biến trang đăng ký tài khoản mới -----------------
    *** Khi sử dụng Attribute để xác định quyền truy cập cho các Razor Page thì trong file Startup.cs
        ta phải cấu hình thêm cho phương thức ConfigureServices
        >>> Ví dụ: muốn truy cập vào trang Pages/Blog/Index thì bắt buộc phải Login
    
       
       
    -------------- Xác thực địa chỉ email khi đăng ký tài khoản ---------------
    
*/