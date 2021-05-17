namespace Coderful.Web.Session
{
	using System.Web;

	/// <summary>
	/// Utility class that manages a session object.
	/// </summary>
	/// <typeparam name="TSessionData">Type of object that will be kept in the session state.</typeparam>
	public class SessionManager<TSessionData> : ISessionManager<TSessionData>
		where TSessionData : class
	{
		private readonly string sessionKey;

		public SessionManager(string sessionKey)
		{
			this.sessionKey = sessionKey;
		}

		/// <summary>
		/// Gets data for the current session.
		/// </summary>
		/// <returns>TSessionData instance or null if no session is present.</returns>
		public TSessionData GetData()
		{
			return HttpContext.Current.Session[this.sessionKey] as TSessionData;
		}

		/// <summary>
		/// Sets data for the current session.
		/// </summary>
		/// <param name="data">TSessionData instance or null.</param>
		public void SetData(TSessionData data)
		{
			HttpContext.Current.Session.Add(this.sessionKey, data);
		}

		/// <summary>
		/// Clear the current session.
		/// </summary>
		public void Clear()
		{
			if (HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session.Remove(this.sessionKey);
			}
		}
	}
}