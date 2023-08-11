using ASP.NET_RazorPage_P8.Models;
using ASP.NET_RazorPage_P8.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ASP.NET_RazorPage_P8
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            //  ---------- Cấu hình chuỗi kết nối, đăng ký DbContext vào DI của ứng dụng ------------
            services.AddDbContext<MyBlogContext>(options =>
            {
                string connectString = Configuration.GetConnectionString("MyBlogContext");
                options.UseSqlServer(connectString);
            });

            //---------Đăng ký các dịch vụ của Identity -----------
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<MyBlogContext>()
                    .AddDefaultTokenProviders();

            // --------- Sử dụng giao diện mặc định (UI) của Identity ----------
            //Đảm bảo có tích hợp package: Microsoft.AspNetCore.Identity.UI
            //Để có thể render được các trang razor page mặc định, như Login, Logout, Sign up...
            //Ví dụ các URL: /Identity/Account/Login, /Identity/Account/Manage,...
            //services.AddDefaultIdentity<AppUser>()
            //        .AddEntityFrameworkStores<MyBlogContext>()
            //        .AddDefaultTokenProviders();


            // --------- Một số thiết lập cho các dịch vụ Identity -------------
            // Truy cập IdentityOptions
            services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); // Khóa 3 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất

                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true;

            });

            // --------- Tạo dịch vụ IMailSender sử dụng bởi Identity ----------
            services.AddOptions();
            var mailsetting = Configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailsetting);
            services.AddSingleton<IEmailSender, SendMailService>();

            // --------- Khi sử dụng Attribute để xác định quyền truy cập cho các Razor Page ---------
            services.ConfigureApplicationCookie(options =>
            {
                // Thiết lập đường dẫn tới trang Login của app
                options.LoginPath = "/login/";
                //Thiết lập đường dẫn tới trang Logout của app
                options.LogoutPath = "/logout/";
                //Thiết lập đường dẫn tới trang Khi User bị cấm truy cập
                options.AccessDeniedPath = "/khongduoctruycap.html";
            });
		}
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            // *********** Sử dụng các dịch vụ của Identity phục vụ cho việc Login, Register
            //Ví dụ: Nếu chưa Login thì hiện url để Login còn Login rồi thì không hiện chẳng hạn...
            //SignInManager<AppUser> s;
            //UserManager<AppUser> u;
            //Từ 2 dịch vụ này ta có thể đọc thông tin từ 1 user, kiểm tra xem có user đăng nhập không
            //user đấy đăng nhập thì thông tin user đăng nhập là gì
            //>>>>>>>>Để sử dụng được 2 dịch vụ này thì chúng ta chỉ việc inject vào các PageModel
            //        giống như inject các dịch vụ khác kể cả ở trong View ta cũng có thể inject

            // >>>>>>> Để inject các dịch vụ vào trực tiếp View của các RazorPage hay các Action Controller...
            //         thì dùng chỉ thị @inject <Kieu-dich-vu-muon-inject> <Ten-dich-vu> 
        }
    }
}
