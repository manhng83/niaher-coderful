namespace Coderful.EntityFramework.Testing.Mock
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Reflection;
	using Moq;

	public class MockedDbContext<TDbContext>
		where TDbContext : DbContext
	{
		private readonly Dictionary<Type, object> dbSets = new Dictionary<Type, object>();

		public MockedDbContext(Mock<TDbContext> dbContextMock, params object[] dbSetMocks)
		{
			this.DbContext = dbContextMock;

			foreach (var dbSetMock in dbSetMocks)
			{
				this.dbSets.Add(dbSetMock.GetType(), dbSetMock);
			}

			// Entity graph support.
			this.DbContext.Setup(t => t.SaveChanges()).Callback(this.SaveChanges);
		}

		public Mock<TDbContext> DbContext { get; set; }

		public Mock<DbSet<TEntity>> GetDbSet<TEntity>() where TEntity : class
		{
			return this.dbSets[typeof(Mock<DbSet<TEntity>>)] as Mock<DbSet<TEntity>>;
		}

		internal object GetDbSet(Type entityType)
		{
			var genericMockType = typeof(Mock<>);
			var genericDbSetType = typeof(DbSet<>);

			var mockType = genericMockType.MakeGenericType(genericDbSetType.MakeGenericType(entityType));

			var mock = this.dbSets.ContainsKey(mockType) ? this.dbSets[mockType] : null;

			return mock;
		}

		private void EnsureCollectionItemsInDbContext(PropertyInfo collectionProperty, object dbSetItem)
		{
			var list = EntityMetadata<TDbContext>.EnumerateProperty(collectionProperty, dbSetItem)
				.Cast<object>()
				.ToList();

			if (!list.Any())
			{
				return;
			}

			var dbSet = new MockManager<TDbContext>(this, collectionProperty.PropertyType.GenericTypeArguments[0]);

			dbSet.EnsureMany(list);
		}

		private void EnsureItemInDbContext(PropertyInfo navigationProperty, object dbSetItem)
		{
			var propertyValue = navigationProperty.GetValue(dbSetItem);

			if (propertyValue == null)
			{
				return;
			}

			var dbSet = new MockManager<TDbContext>(this, navigationProperty.PropertyType);

			dbSet.Ensure(propertyValue);
		}

		private void SaveChanges()
		{
			foreach (var dbSet in this.dbSets)
			{
				var entityType = dbSet.Value.GetType().GenericTypeArguments[0].GenericTypeArguments[0];
				var dbSetMeta = new MockManager<TDbContext>(this, entityType);

				foreach (var dbSetItem in dbSetMeta.GetDbSetItems())
				{
					foreach (var collectionProperty in dbSetMeta.Metadata.EntityType.CollectionProperties)
					{
						this.EnsureCollectionItemsInDbContext(collectionProperty, dbSetItem);
					}

					foreach (var navigationProperty in dbSetMeta.Metadata.EntityType.NavigationProperties)
					{
						this.EnsureItemInDbContext(navigationProperty, dbSetItem);
					}
				}
			}
		}
	}
}