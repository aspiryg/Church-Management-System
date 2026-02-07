using ChurchMS_Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChurchMS_Backend.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; } 
    }
}
