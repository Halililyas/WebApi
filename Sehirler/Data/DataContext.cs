using Microsoft.EntityFrameworkCore;
using Sehirler.Models;

namespace Sehirler.Data
{
	public class DataContext:DbContext
	{
		public DataContext(DbContextOptions options) : base(options)
		{

		}
        public DbSet<Value> Values { get; set; }
		public DbSet<City> Cities { get; set; }

		public DbSet<User> Users  { get; set; }

		public DbSet<Photo> Photos { get; set; }
	}
}
