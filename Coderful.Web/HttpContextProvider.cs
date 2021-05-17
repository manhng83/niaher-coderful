namespace Coderful.Web
{
	using System.Web;
	using Coderful.Core.Provider;

	/// <summary>
	/// IProvider for objects associated with the current HTTP context.
	/// </summary>
	/// <typeparam name="TContext">Type of object to provide.</typeparam>
	public class HttpContextProvider<TContext> : IProvider<TContext> 
		where TContext : class, new()
	{
		private readonly string key;

		/// <summary>
		/// Initializes a new instance of the HttpContextProvider class.
		/// </summary>
		/// <param name="key">Unique key. Having multiple instances of HttpContextProvider with the same key will result in accessing the same object instance.</param>
		public HttpContextProvider(string key)
		{
			this.key = key;
		}

		/// <summary>
		/// Gets object associated with the current HTTP context.
		/// </summary>
		/// <returns>TContext instance.</returns>
		/// <remarks>If no TContext instance is found, then a new one will be created and associated with the current HTTP context.</remarks>
		public TContext Get()
		{
			var context = HttpContext.Current.Items[key] as TContext;

			if (context == null)
			{
				HttpContext.Current.Items[key] = context = new TContext();
			}

			return context;
		}
	}
}