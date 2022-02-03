using JobsityChatAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobsityChatAPI.Data
{
    public class BaseContext : IdentityDbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options) { }

        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalUser>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<LocalUser>().HasIndex(u => u.UserName).IsUnique();
            base.OnModelCreating(modelBuilder);
        }
    }
}
