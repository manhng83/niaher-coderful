namespace Coderful.EntityFramework.Testing.Test.Mocking
{
	using System.Collections.Generic;
	using Coderful.EntityFramework.Testing.Mock;
	using Coderful.EntityFramework.Testing.Test.DbContext;
	using Moq;

	internal static class MyMoqUtilities
	{
		public static MockedDbContext<MyDbContext> MockDbContext(
			IList<Contract> contracts = null,
			IList<User> users = null)
		{
			var mockContext = new Mock<MyDbContext>();

			// Create the DbSet objects.
			var dbSets = new object[]
			{
				mockContext.MockDbSet(contracts, (objects, contract) => contract.ContractId == (int)objects[0] && contract.AmendmentId == (int)objects[1]),
				mockContext.MockDbSet(users, (objects, user) => user.Id == (int)objects[0])
			};

			return new MockedDbContext<MyDbContext>(mockContext, dbSets);
		}
	}
}