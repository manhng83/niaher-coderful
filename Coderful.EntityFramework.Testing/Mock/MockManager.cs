namespace Coderful.EntityFramework.Testing.Mock
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data.Entity;

	internal class MockManager<TDbContext>
		where TDbContext : DbContext
	{
		public MockManager(MockedDbContext<TDbContext> dbContext, Type entityType)
		{
			this.Metadata = DbSetMetadata<TDbContext>.GetByEntityType(dbContext, entityType);

			this.MockObject = dbContext.GetDbSet(entityType);

			if (this.MockObject == null)
			{
				throw new Exception("Mock for " + entityType.FullName + " is missing.");
			}

			this.DbSet = this.Metadata.MockType.GetProperty("Object", typeof(DbSet<>)).GetValue(this.MockObject);
		}

		public object DbSet { get; set; }
		public DbSetMetadata<TDbContext> Metadata { get; set; }
		public object MockObject { get; set; }

		public void AddToDbSet(object item)
		{
			var addMethodInfo = this.Metadata.DbSetType.GetMethod("Add");
			addMethodInfo.Invoke(this.DbSet, new[] { item });
		}

		public bool ContainsItem(object item)
		{
			foreach (var dbSetItem in this.GetDbSetItems())
			{
				if (dbSetItem == item)
				{
					return true;
				}
			}

			return false;
		}

		public void Ensure(object entity)
		{
			if (entity != null && !this.ContainsItem(entity))
			{
				this.AddToDbSet(entity);
			}
		}

		public void EnsureMany(IEnumerable<object> entities)
		{
			foreach (var listItem in entities)
			{
				this.Ensure(listItem);
			}
		}

		public IEnumerable<object> GetDbSetItems()
		{
			var set = this.DbSet;
			var expression = set.GetType().GetProperty("Expression").GetValue(set);
			var list = expression.GetType().GetProperty("Value").GetValue(expression);

			foreach (var item in (IEnumerable)list)
			{
				yield return item;
			}
		}
	}
}