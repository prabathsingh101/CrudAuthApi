using Auth.API.Models.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<TokenInfo> TokenInfo { get; set; }

        //public DbSet<Category> Categories { get; set; }

        public DbSet<Student> Students { get; set; }  
        
        public  DbSet<ImageModel> Images { get; set; }  

        public DbSet<TeacherModel> Teachers { get; set; }   

        public DbSet<UserModel> GetAllUsers {  get; set; }
    }
}
