namespace Coderful.EntityFramework.Testing.Test
{
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Threading.Tasks;
	using Coderful.EntityFramework.Testing.Test.DbContext;
	using Coderful.EntityFramework.Testing.Test.Mocking;
	using FluentAssertions;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class MockTest
	{
		[TestMethod]
		public async Task AsyncWorks()
		{
			// Create test data.
			var contracts = new List<Contract>
			{
				new Contract(1, 0),
				new Contract(2, 1)
			};

			var users = new List<User>
			{
				new User("John"),
				new User("Jane")
			};

			// Create DbContext with the predefined test data.
			var mock = MyMoqUtilities.MockDbContext(
				contracts: contracts,
				users: users).DbContext;

			// FirstOrDefaultAsync.
			var user = await mock.Object.Users.FirstOrDefaultAsync(t => t.Name == "John");
			user.Should().NotBeNull();
			user.Name.Should().Be("John");

			// ToListAsync.
			var allContracts = await mock.Object.Contracts.ToListAsync();
			allContracts.Count.Should().Be(2);
		}

		[TestMethod]
		public void DbContextWorks()
		{
			// Create test data.
			var contracts = new List<Contract>
			{
				new Contract(1, 0),
				new Contract(2, 1)
			};

			var users = new List<User>
			{
				new User("John"),
				new User("Jane")
			};

			// Create DbContext with the predefined test data.
			var mock = MyMoqUtilities.MockDbContext(
				contracts: contracts,
				users: users).DbContext;

			var dbContext = mock.Object;

			// Add.
			var batman = new User("Batman");
			dbContext.Users.Add(batman);
			dbContext.Users.Count(u => u.Name == "Batman").Should().Be(1, "Can add new record");

			// Remove.
			dbContext.Users.Remove(batman);
			dbContext.Users.Count(u => u.Name == "Batman").Should().Be(0, "Can remove record");

			// Query.
			dbContext.Users.Single(u => u.Name == "John").Name.Should().Be("John", "Can query");

			// ToList.
			dbContext.Users.ToList().Count.Should().Be(2);

			// Save changes won't actually do anything, since all the data is kept in memory.
			// This should be ideal for unit-testing purposes.
			dbContext.SaveChanges();
			dbContext.Users.Count().Should().Be(2);
		}
	}
}