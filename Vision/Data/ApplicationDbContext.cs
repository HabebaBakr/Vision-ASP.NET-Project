using Microsoft.EntityFrameworkCore;
using Vision.Models;

namespace Vision.Data
{
	public class ApplicationDbContext : DbContext
	{

        public DbSet<Book> books { get; set; }

        public DbSet<Category> categories { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
	}
}
