namespace Coderful.Web.Authentication
{
	/// <summary>
	/// Describes a service that performs persistent authentication over HTTP.
	/// </summary>
	/// <typeparam name="TCredentials">Type of credentials to authenticate.</typeparam>
	public interface ILoginService<in TCredentials>
	{
		/// <summary>
		/// Attempts to perform a login.
		/// </summary>
		/// <param name="credentials">LoginService credentials.</param>
		/// <returns>Success or failure.</returns>
		/// <remarks>This method will result in a non-persistent login (a login that only lasts the lifetime
		/// of a browser session).</remarks>
		bool Login(TCredentials credentials);

		/// <summary>
		/// Attempts to perform a login.
		/// </summary>
		/// <param name="credentials">LoginService credentials.</param>
		/// <param name="persistLogin">True to create persistent cookie (one that is saved across browser sessions).</param>
		/// <returns>Success or failure.</returns>
		bool Login(TCredentials credentials, bool persistLogin);

		/// <summary>
		/// Performs a logout for the current user.
		/// </summary>
		void Logout();

		/// <summary>
		/// Gets a value indicating whether current request is authenticated or not.
		/// </summary>
		/// <returns>True or false.</returns>
		bool IsRequestAuthenticated();

		/// <summary>
		/// Authenticates current request.
		/// </summary>
		void AuthenticateRequest();

		/// <summary>
		/// Validates the given credentials.
		/// </summary>
		/// <param name="credentials">TCredentials instance.</param>
		/// <returns>True if credentials are valid, false otherwise.</returns>
		bool ValidateCredentials(TCredentials credentials);
	}
}