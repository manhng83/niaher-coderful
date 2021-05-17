namespace Coderful.EntityFramework.Testing.Mock
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Moq;

	public static class MoqExtensions
	{
		/// <summary>
		/// Sets up <see cref="DbSet"/> property in DbContext with a mocked <see cref="DbSet"/>. The mocked <see cref="DbSet"/> 
		/// is initialized with the provided data. The resulting <see cref="DbSet"/> will work in memory, never touching the database.
		/// </summary>
		/// <typeparam name="TEntity">Type of entities that <see cref="DbSet"/> holds.</typeparam>
		/// <typeparam name="TDbContext">Type of <see cref="DbContext"/>.</typeparam>
		/// <param name="mockContext">Mock of <see cref="TDbContext"/>, with which to link the newly created <see cref="DbSet"/> mock.</param>
		/// <param name="data">Data to setup the <see cref="DbSet"/> with. In case of null, an empty <see cref="DbSet"/> will be setup.</param>
		/// <param name="isMatch">Function which will check equality based on primary key of <see cref="TEntity"/>.</param>
		/// <returns>The instance of <see cref="Mock"/>, which was linked to the <see cref="mockContext"/>.</returns>
		public static Mock<DbSet<TEntity>> MockDbSet<TEntity, TDbContext>(
			this Mock<TDbContext> mockContext,
			IList<TEntity> data,
			Func<object[], TEntity, bool> isMatch)
			where TEntity : class
			where TDbContext : DbContext
		{
			var dbSet = MoqUtilities.MockDbSet(data ?? new List<TEntity>(), isMatch);
			mockContext.LinkDbSet(dbSet);

			return dbSet;
		}

		public static Mock<DbSet<T>> SetupDerivedEntity<T, TDerived>(this Mock<DbSet<T>> mock)
			where T : class
			where TDerived : class, T
		{
			mock.Setup(m => m.Create<TDerived>()).Returns(MoqUtilities.InstantiateObject<TDerived>());
			return mock;
		}

		private static void LinkDbSet<TEntity, TDbContext>(this Mock<TDbContext> mockContext, IMock<DbSet<TEntity>> dbSet)
			where TEntity : class
			where TDbContext : DbContext
		{
			var dbSetProperty = mockContext.Object.GetType().GetProperties()
				.SingleOrDefault(f => f.MemberType == MemberTypes.Property && f.PropertyType.UnderlyingSystemType.IsAssignableFrom(typeof(DbSet<TEntity>)));

			if (dbSetProperty != null)
			{
				// Build Expression<Func<TDbContext, DbSet<TEntity>>>.
				ParameterExpression parameter = Expression.Parameter(typeof(TDbContext), "i");
				MemberExpression property = Expression.Property(parameter, dbSetProperty.Name);
				var dbSetType = typeof(DbSet<>).MakeGenericType(typeof(TEntity));
				var delegateType = typeof(Func<,>).MakeGenericType(typeof(TDbContext), dbSetType);
				var expression = Expression.Lambda(delegateType, property, parameter) as Expression<Func<TDbContext, DbSet<TEntity>>>;

				// Link dbSet.
				mockContext.Setup(expression).Returns(dbSet.Object);
			}

			// Link generic Set<TEntity> as well.
			mockContext.Setup(m => m.Set<TEntity>()).Returns(dbSet.Object);
		}
	}
}