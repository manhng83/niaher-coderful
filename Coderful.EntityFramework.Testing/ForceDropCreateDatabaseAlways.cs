namespace Coderful.EntityFramework.Testing
{
	using System.Data.Entity;

	public class ForceDropCreateDatabaseAlways<T> : DropCreateDatabaseAlways<T>
		where T : DbContext
	{
		public override void InitializeDatabase(T context)
		{
			var sql = string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE", context.Database.Connection.Database);

			context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql);

			base.InitializeDatabase(context);
		}
	}
}