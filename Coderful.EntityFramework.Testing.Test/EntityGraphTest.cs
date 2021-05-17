namespace Coderful.EntityFramework.Testing.Test
{
	using System.Collections.Generic;
	using System.Linq;
	using Coderful.EntityFramework.Testing.Test.DbContext;
	using Coderful.EntityFramework.Testing.Test.Mocking;
	using FluentAssertions;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestStack.BDDfy;

	[TestClass]
	public class EntityGraphTest
	{
		[TestMethod]
		public void CollectionPropertiesAreTraversed()
		{
			new Scenario()
				.Given(s => s.UserIsAdded("john"))
				.When(s => s.ContractAddedToUser(1, 2))
				.And(s => s.ChangesAreSaved())
				.Then(s => s.ContractIsInDbContext(1, 2))
				.BDDfy();
		}

		[TestMethod]
		public void ForeignKeyPropertiesAreTraversed()
		{
			new Scenario()
				.Given(s => s.ContractIsCreated(2, 5))
				.When(s => s.UserAssignedToContract("john"))
				.And(s => s.ChangesAreSaved())
				.Then(s => s.UserIsInDbContext("john"))
				.BDDfy();
		}

		public class Scenario
		{
			private readonly MyDbContext context;
			private Contract contract;
			private User user;

			public Scenario()
			{
				this.context = MyMoqUtilities.MockDbContext().DbContext.Object;
			}

			public void ChangesAreSaved()
			{
				this.context.SaveChanges();
			}

			public void ContractAddedToUser(int id, int amendmentId)
			{
				this.user.Contracts.Add(new Contract(id, amendmentId));
			}

			public void ContractCountShouldBe(int count)
			{
				this.context.Contracts.Count().Should().Be(count);
			}

			public void ContractIsCreated(int id, int amendmentId)
			{
				this.contract = new Contract(id, amendmentId);
				this.context.Contracts.Add(this.contract);
			}

			public void ContractIsInDbContext(int id, int amendmentId)
			{
				this.context.Contracts.Any(c => c.ContractId == id && c.AmendmentId == amendmentId).Should().BeTrue();
			}

			public void UserAssignedToContract(string name)
			{
				this.contract.ContractHolder = new User(name);
			}

			public void UserCountShouldBe(int count)
			{
				this.context.Users.Count().Should().Be(count);
			}

			public void UserIsAdded(string name)
			{
				this.user = new User(name)
				{
					Contracts = new List<Contract>()
				};

				this.context.Users.Add(this.user);
			}

			public void UserIsInDbContext(string name)
			{
				this.context.Users.Any(c => c.Name == name).Should().BeTrue();
			}
		}
	}
}