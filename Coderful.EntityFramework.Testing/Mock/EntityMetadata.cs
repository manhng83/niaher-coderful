namespace Coderful.EntityFramework.Testing.Mock
{
	using System;
	using System.Collections;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Reflection;

	internal class EntityMetadata<TDbContext>
		where TDbContext : DbContext
	{
		private static readonly ConcurrentDictionary<Type, EntityMetadata<TDbContext>> EntityTypeCache =
			new ConcurrentDictionary<Type, EntityMetadata<TDbContext>>();

		private readonly Lazy<List<Type>> dbSetEntityTypes = new Lazy<List<Type>>(() =>
		{
			return typeof(TDbContext).GetProperties()
				.Where(p => p.PropertyType.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDbSet<>)))
				.Select(t => t.PropertyType.GenericTypeArguments[0])
				.ToList();
		});

		private PropertyInfo[] collectionProperties;
		private PropertyInfo[] entityTypeNavigationProperties;
		private PropertyInfo[] entityTypeProperties;

		private EntityMetadata()
		{
		}

		public PropertyInfo[] CollectionProperties
		{
			get
			{
				if (this.collectionProperties == null)
				{
					this.collectionProperties = this.Properties
						.Where(t => t.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
						.Where(t => this.dbSetEntityTypes.Value.Any(e => e == t.PropertyType.GenericTypeArguments[0]))
						.ToArray();
				}

				return this.collectionProperties;
			}
		}

		public PropertyInfo[] NavigationProperties
		{
			get
			{
				if (this.entityTypeNavigationProperties == null)
				{
					this.entityTypeNavigationProperties = this.Properties
						.Where(t => !t.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
						.Where(t => this.dbSetEntityTypes.Value.Any(e => e == t.PropertyType))
						.ToArray();
				}

				return this.entityTypeNavigationProperties;
			}
		}

		public Type Type { get; set; }

		private IEnumerable<PropertyInfo> Properties
		{
			get
			{
				if (this.entityTypeProperties == null)
				{
					this.entityTypeProperties = this.Type
						.GetProperties()
						.Where(p => !p.PropertyType.IsValueType && p.PropertyType != typeof(string))
						.ToArray();
				}

				return this.entityTypeProperties;
			}
		}

		public static IEnumerable EnumerateProperty(PropertyInfo propertyType, object item)
		{
			if (item != null)
			{
				var propertyValue = propertyType.GetValue(item);

				if (propertyValue != null)
				{
					foreach (var i in (IEnumerable)propertyValue)
					{
						yield return i;
					}
				}
			}
		}

		public static EntityMetadata<TDbContext> Get(MockedDbContext<TDbContext> dbContext, Type type)
		{
			return EntityTypeCache.GetOrAdd(type, type1 => new EntityMetadata<TDbContext>
			{
				Type = type
			});
		}
	}
}