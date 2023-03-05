using Microsoft.EntityFrameworkCore;

namespace CkEditor.Models
{
	public class AppDb : DbContext
	{
		public AppDb(DbContextOptions options) : base(options)
		{

		}
		public DbSet<Image> Images { get; set; }
	}
}
