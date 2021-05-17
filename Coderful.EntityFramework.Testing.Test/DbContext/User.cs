namespace Coderful.EntityFramework.Testing.Test.DbContext
{
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class User
	{
		public User(string name)
		{
			this.Name = name;
		}

		public virtual ICollection<Contract> Contracts { get; set; }

		[Key]
		public int Id { get; set; }

		public string Name { get; set; }
	}
}