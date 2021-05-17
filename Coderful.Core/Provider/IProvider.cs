namespace Coderful.Core.Provider
{
	/// <summary>
	/// Interface for a provider.
	/// </summary>
	/// <typeparam name="T">Type of item being provided.</typeparam>
	public interface IProvider<out T>
	{
		/// <summary>
		/// Retrieves current instance.
		/// </summary>
		/// <returns>Instance of T.</returns>
		T Get();
	}
}
