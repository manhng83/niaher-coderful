namespace Coderful.Web.Mvc
{
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;
	using System.Web.Http;
	using global::Breeze.ContextProvider;
	using global::Breeze.ContextProvider.EF6;
	using global::Breeze.WebApi2;
	using Coderful.Core.Enums;
	using Newtonsoft.Json.Linq;

	[BreezeController]
	public abstract class BreezeController<T> : ApiController
		where T : DbContext, new()
	{
		protected readonly EFContextProvider<T> ContextProvider = new EFContextProvider<T>();

		protected BreezeController()
			: this(new EFContextProvider<T>())
		{
		}

		protected BreezeController(EFContextProvider<T> contextProvider)
		{
			this.ContextProvider = contextProvider;
		}

		// ReSharper disable once StaticFieldInGenericType
		private static readonly IDictionary<string, IEnumerable<EnumValue<long>>> EnumLists = new Dictionary<string, IEnumerable<EnumValue<long>>>();

		[HttpGet]
		public string Metadata()
		{
			return this.ContextProvider.Metadata();
		}

		[HttpPost]
		public virtual SaveResult SaveChanges(JObject saveBundle)
		{
			return this.ContextProvider.SaveChanges(saveBundle);
		}

		[HttpGet]
		public IEnumerable<EnumValue<long>> Enums(string type)
		{
			return EnumLists[type];
		}

		protected static void RegisterEnum<TEnum>()
			where TEnum : struct, IConvertible
		{
			var enumUnderlyingType = typeof(TEnum).GetEnumUnderlyingType();

			if (enumUnderlyingType == typeof(byte))
			{
				EnumLists[typeof(TEnum).Name] = EnumUtility.GetEnumValues<TEnum, byte>().Select(v => new EnumValue<long>(v.Key, v.Name, v.Description));
			}
			else if (enumUnderlyingType == typeof(int))
			{
				EnumLists[typeof(TEnum).Name] = EnumUtility.GetEnumValues<TEnum, int>().Select(v => new EnumValue<long>(v.Key, v.Name, v.Description));
			}
			else
			{
				EnumLists[typeof(TEnum).Name] = EnumUtility.GetEnumValues<TEnum, long>().Select(v => new EnumValue<long>(v.Key, v.Name, v.Description));
			}
		}
	}
}
