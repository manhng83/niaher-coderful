namespace Coderful.EntityFramework.Testing.Mock
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Data.Entity;
	using System.Data.Entity.Infrastructure;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Threading.Tasks;
	using Coderful.EntityFramework.Testing.DbAsync;
	using Moq;

	public class MoqUtilities
	{
		/// <summary>
		/// Returns a mock of DbSet.
		/// </summary>
		/// <typeparam name="T">Type of entity stored in the DbSet.</typeparam>
		/// <param name="data">Initial list of items.</param>
		/// <param name="isMatch">Function that will be used to locate items by primary key.</param>
		/// <returns>Mock object for the DbSet.</returns>
		public static Mock<DbSet<T>> MockDbSet<T>(IList<T> data, Func<object[], T, bool> isMatch) where T : class
		{
			var mockSet = new Mock<DbSet<T>>();
			SetupDbSet(mockSet, data);

			mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(id => data.SingleOrDefault(u => isMatch(id, u)));
			mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(id => Task.FromResult(data.SingleOrDefault(u => isMatch(id, u))));

			// Add and attach both add item to the set.
			mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
			mockSet.Setup(m => m.Attach(It.IsAny<T>())).Callback<T>(data.Add);

			mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(items =>
			{
				foreach (var item in items)
				{
					data.Add(item);
				}
			});

			mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(item => data.Remove(item));
			mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(items =>
			{
				foreach (var item in items)
				{
					data.Remove(item);
				}
			});

			mockSet.Setup(m => m.Create()).Returns(InstantiateObject<T>);
			mockSet.Setup(m => m.Create<T>()).Returns(InstantiateObject<T>);

			mockSet.Setup(m => m.Local).Returns(new ObservableCollection<T>(data));

			// Support for `Include()`.
			mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

			return mockSet;
		}

		/// <summary>
		/// Returns a mock of DbSet for an abstract class. This is useful if you are implementing table-per-hierarchy (TPH)
		/// mapping.
		/// </summary>
		/// <typeparam name="T">Abstract class from which all the derived TPH classes will inherit.</typeparam>
		/// <param name="data">Initial list of items.</param>
		/// <param name="isMatch">Function that will be used to locate items by primary key.</param>
		/// <returns>Mock object for the DbSet.</returns>
		public static Mock<DbSet<T>> MockDbSetForAbstractEntity<T>(IList<T> data, Func<object[], T, bool> isMatch)
			where T : class
		{
			var mockSet = new Mock<DbSet<T>>();
			SetupDbSet(mockSet, data);

			mockSet.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(id => data.SingleOrDefault(u => isMatch(id, u)));
			mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>())).Returns<object[]>(id => Task.FromResult(data.SingleOrDefault(u => isMatch(id, u))));

			// Add and attach both add item to the set.
			mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
			mockSet.Setup(m => m.Attach(It.IsAny<T>())).Callback<T>(data.Add);

			mockSet.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(items =>
			{
				foreach (var item in items)
				{
					data.Add(item);
				}
			});

			mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(item => data.Remove(item));
			mockSet.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(items =>
			{
				foreach (var item in items)
				{
					data.Remove(item);
				}
			});

			mockSet.Setup(m => m.Create()).Returns(() =>
			{
				var message = string.Format("Instantiating '{0}' is not supported.", typeof(T).FullName);
				throw new NotSupportedException(message);
			});

			return mockSet;
		}

		public static void SetupDbSet<T>(Mock<DbSet<T>> mockSet, IList<T> data) where T : class
		{
			var queryable = data.AsQueryable();

			// The below is needed to support `ToList()` calls and async support.
			mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));
			mockSet.As<IDbAsyncEnumerable<T>>().Setup(m => m.GetAsyncEnumerator()).Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));
			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);
		}

		internal static T InstantiateObject<T>() where T : class
		{
			var constructor = typeof(T).GetConstructor(Type.EmptyTypes);
			if (constructor != null)
			{
				return (T)constructor.Invoke(new object[0]);
			}

			return (T)FormatterServices.GetUninitializedObject(typeof(T));
		}
	}
}