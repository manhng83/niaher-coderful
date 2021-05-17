namespace Coderful.EntityFramework.Testing.Test.DbContext
{
	using System.Data.Entity;

	public class MyDbContext : DbContext
	{
		public virtual DbSet<Contract> Contracts { get; set; }
		public virtual DbSet<User> Users { get; set; }
	}
}