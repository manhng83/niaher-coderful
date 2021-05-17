namespace Coderful.EntityFramework.Testing.Mock
{
	using System;
	using System.Collections.Concurrent;
	using System.Data.Entity;
	using Moq;

	internal class DbSetMetadata<TDbContext>
		where TDbContext : DbContext
	{
		private static readonly ConcurrentDictionary<Type, DbSetMetadata<TDbContext>> EntityTypeCache =
			new ConcurrentDictionary<Type, DbSetMetadata<TDbContext>>();

		private DbSetMetadata()
		{
		}

		public Type DbSetType { get; set; }
		public EntityMetadata<TDbContext> EntityType { get; set; }
		public Type MockType { get; set; }

		public static DbSetMetadata<TDbContext> GetByEntityType(MockedDbContext<TDbContext> dbContext, Type entityType)
		{
			return EntityTypeCache.GetOrAdd(entityType, type =>
			{
				var mockDbSet = dbContext.GetDbSet(entityType);

				if (mockDbSet == null)
				{
					return null;
				}

				// DbSet<TEntity>
				var dbSetType = typeof(DbSet<>).MakeGenericType(entityType);

				// Mock<DbSet<TEntity>>
				var mockType = typeof(Mock<>).MakeGenericType(dbSetType);

				return new DbSetMetadata<TDbContext>
				{
					DbSetType = dbSetType,
					MockType = mockType,
					EntityType = EntityMetadata<TDbContext>.Get(dbContext, entityType)
				};
			});
		}
	}
}