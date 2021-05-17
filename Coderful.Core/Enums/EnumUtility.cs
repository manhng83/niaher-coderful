namespace Coderful.Core.Enums
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Humanizer;

	public static class EnumUtility
	{
		public static IEnumerable<EnumValue<TUnderlying>> GetEnumValues<TEnum, TUnderlying>()
			where TEnum : struct, IConvertible
			where TUnderlying : struct, IConvertible
		{
			EnumEnforcer.EnforceIsEnum<TEnum>("TEnum", "GetList");

			var enumType = typeof(TEnum);
			var result = new List<EnumValue<TUnderlying>>();
			
			foreach (object value in Enum.GetValues(enumType))
			{
				var key = (TUnderlying)value;
				var name = value.ToString();
				
				// ReSharper disable once PossibleInvalidCastException
				var description = ((Enum)value).Humanize();

				var enumValue = new EnumValue<TUnderlying>(key, name, description);
				result.Add(enumValue);
			}

			return result;
		}

		public static IList<T> GetAllValues<T>()
		{
			return Enum.GetValues(typeof(T)).OfType<T>().ToList();
		}
	}
}
