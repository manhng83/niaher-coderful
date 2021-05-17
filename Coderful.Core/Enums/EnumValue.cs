namespace Coderful.Core.Enums
{
	using System;

	/// <summary>
	/// Represents a single value in an enumeration.
	/// </summary>
	/// <typeparam name="TKey">Underlying type of the enum (e.g. - byte, int, long).</typeparam>
	public class EnumValue<TKey>
		where TKey : struct, IConvertible
	{
		public EnumValue(TKey key, string name)
			: this(key, name, name)
		{
		}

		public EnumValue(TKey key, string name, string description)
		{
			this.Key = key;
			this.Name = name;
			this.Description = description;
		}

		public TKey Key { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
	}
}