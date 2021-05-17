namespace Coderful.Web.Session
{
	/// <summary>
	/// Describes utility class that manages a session object.
	/// </summary>
	/// <typeparam name="TSessionData">Type of object that will be kept in the session state.</typeparam>
	public interface ISessionManager<TSessionData>
	{
		/// <summary>
		/// Gets data for the current session.
		/// </summary>
		/// <returns>TSessionData instance or null if no session is present.</returns>
		TSessionData GetData();

		/// <summary>
		/// Sets data for the current session.
		/// </summary>
		/// <param name="data">TSessionData instance or null.</param>
		void SetData(TSessionData data);

		/// <summary>
		/// Clear the current session.
		/// </summary>
		void Clear();
	}
}