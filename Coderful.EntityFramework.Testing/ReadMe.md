*Coderful.EntityFramework.Testing* allows you to mock `DbContext`, which can be useful for unit-testing. This library only supports EF6.

## Why you need this library?
There's quite a lot of boilerplate code involved in setting up DbContext mocking. To avoid writing the same code for each project you can use this library.

## How to use?

First let's assume that we have this `DbContext`:

```
public class MyDbContext : DbContext
{
	public virtual DbSet<User> Users { get; set; }
	public virtual DbSet<Contract> Contracts { get; set; }
}
```

To mock this `DbContext`, install [the NuGet package][1] inside your unit-test project:

```
Install-Package Coderful.EntityFramework.Testing
```

Then create `MyMoqUtilities` class (also inside the unit-test project), which should look something like this:

```
using System.Collections.Generic;
using Coderful.EntityFramework.Testing.Mock;
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
```

Now you're all set and ready to create the mocks of your `DbContext`. The code is really simple:

```
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
var dbContext = MyMoqUtilities.MockDbContext(
	contracts: contracts,
	users: users).DbContext.Object;
```

That's it. Now you can use the `dbContext` object as if it was a standard `DbContext` instance:

```
// Add.
dbContext.Users.Add(newUser);

// Remove.
dbContext.Users.Remove(someUser);

// Query.
var john = dbContext.Users.Where(u => u.Name == "John");

// Save changes won't actually do anything, since all the data is kept in memory.
// This should be ideal for unit-testing purposes.
dbContext.SaveChanges();
```

## Dependencies

* [Moq][moq]
* [Entity Framework 6][ef]

[moq]:https://github.com/Moq/moq4
[0]:https://bitbucket.org/niaher/coderful-corebits/src/b973835dcb7ba7c77c5e15381adf4209d1f5721d/Coderful.EntityFramework.Testing/?at=master
[1]:https://www.nuget.org/packages/Coderful.EntityFramework.Testing
[ef]:https://www.nuget.org/packages/EntityFramework/6.1.1