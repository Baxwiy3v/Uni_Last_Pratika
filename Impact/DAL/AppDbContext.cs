using Impact.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Impact.DAL
{
	public class AppDbContext:IdentityDbContext<AppUser>
	{
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Portfolio> Portfolios { get; set; }

		public DbSet<Service> Services { get; set; }

        public DbSet<Setting> Settings { get; set; }
    }
}
