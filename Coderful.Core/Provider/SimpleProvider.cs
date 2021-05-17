namespace Coderful.Core.Provider
{
	/// <summary>
	/// Simple IProvider, which returns a new instance each time.
	/// </summary>
	/// <typeparam name="T">Type of item being returned.</typeparam>
	public class SimpleProvider<T> : IProvider<T>
		where T : new()
	{
		/// <summary>
		/// Creates a new instance and returns it.
		/// </summary>
		/// <returns>Instance of T.</returns>
		public T Get()
		{
			return new T();
		}
	}
}