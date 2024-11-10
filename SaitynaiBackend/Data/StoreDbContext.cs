using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaitynaiBackend.Data.Models;

namespace SaitynaiBackend.Data
{
    public class StoreDbContext : IdentityDbContext<StoreUser>
    {
        public StoreDbContext() { }
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<StoreUser> Users { get; set; }
    }
}
