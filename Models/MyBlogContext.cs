using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_RazorPage_P8.Models
{
    public class MyBlogContext : IdentityDbContext<AppUser>
    {
        public MyBlogContext(DbContextOptions<MyBlogContext> options) : base(options)
        {

        }

        public DbSet<Article> articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //--------- Thực hiện ef migration cập nhật CSDL Identity ---------
            // ****Xử lí bỏ tiền tố AspNet_ ở các Table của IdentityDbContext
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }    
            }    
         
        }
    }
}
