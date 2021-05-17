namespace Coderful.Web.Authentication
{
	using System;
	using System.Security.Cryptography;
	using System.Security.Principal;
	using System.Web;
	using System.Web.Security;
	using Newtonsoft.Json;

	/// <summary>
	/// Abstract class providing simple authentication and login mechanism. For authenticated
	/// users, the class will allow storing additional data in the session state.
	/// </summary>
	/// <typeparam name="TCredentials">Type of the credentials.</typeparam>
	/// <typeparam name="TPrincipal">Type of the session data.</typeparam>
	public abstract class LoginService<TCredentials, TPrincipal> : ILoginService<TCredentials>
		where TPrincipal : class, IPrincipal
	{
		/// <summary>
		/// Gets or sets name of the authentication cookie.
		/// </summary>
		private string authenticationCookieName = FormsAuthentication.FormsCookieName;

		/// <summary>
		/// Gets or sets timeout (in minutes) of the authentication cookie.
		/// </summary>
		private int authenticationCookieTimeout = FormsAuthentication.Timeout.Minutes;

		/// <summary>
		/// Gets or sets timeout (in minutes) of the authentication cookie.
		/// </summary>
		/// <remarks>The default value is taken from FormAuthentication.Timeout.Minutes.</remarks>
		public int AuthenticationCookieTimeout
		{
			get
			{
				return this.authenticationCookieTimeout;
			}

			protected set
			{
				this.authenticationCookieTimeout = value;
			}
		}

		/// <summary>
		/// Gets or sets name of the authentication cookie.
		/// </summary>
		/// <remarks>The default value is taken from FormAuthentication.FormsCookieName.</remarks>
		public string AuthenticationCookieName
		{
			get
			{
				return this.authenticationCookieName;
			}

			protected set
			{
				this.authenticationCookieName = value;
			}
		}

		/// <summary>
		/// Attempts to authenticate the credentials and if successful, performs the login.
		/// </summary>
		/// <param name="credentials">Credentials against which to attempt authentication.</param>
		/// <returns>Success or failure.</returns>
		public bool Login(TCredentials credentials)
		{
			return this.Login(credentials, false);
		}

		/// <summary>
		/// Attempts to authenticate the credentials and if successful, performs the login.
		/// </summary>
		/// <param name="credentials">LoginService credentials.</param>
		/// <param name="persistLogin">True to create persistent cookie (one that is saved across browser sessoins).</param>
		/// <returns>Success or failure.</returns>
		public bool Login(TCredentials credentials, bool persistLogin)
		{
			if (this.IsRequestAuthenticated())
			{
				throw new InvalidOperationException(
					"Current request is already authenticated, meaning that the user is already logged in. " +
					"Login cannot be performed while the previous session is still active. Please logout," +
					" before trying to login again.");
			}

			// If authentication is successful.
			bool credentialsAreValid = this.ValidateCredentials(credentials);

			if (credentialsAreValid)
			{
				var userData = this.GetPrincipal(credentials);
				var userDataSerialized = JsonConvert.SerializeObject(userData);

				var authenticationTicket = new FormsAuthenticationTicket(
					2,
					userData.Identity.Name,
					DateTime.Now,
					DateTime.Now.AddMinutes(this.AuthenticationCookieTimeout),
					persistLogin,
					userDataSerialized);

				// Send authentication cookie to the client.
				string encryptedTicket = FormsAuthentication.Encrypt(authenticationTicket);
				HttpCookie authCookie = new HttpCookie(this.AuthenticationCookieName, encryptedTicket);
				HttpContext.Current.Response.Cookies.Add(authCookie);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Performs a logout for the current user.
		/// </summary>
		public void Logout()
		{
			var cookie = HttpContext.Current.Response.Cookies[this.authenticationCookieName];
			if (cookie != null)
			{
				cookie.Expires = DateTime.UtcNow.AddYears(-10);
			}
		}

		/// <summary>
		/// Gets a value indicating whether current request is authenticated.
		/// </summary>
		/// <returns>True or false.</returns>
		/// <remarks>For this method to return a valid value each request must first be authenticated
		/// using LoginService.AuthenticateRequest method.</remarks>
		public bool IsRequestAuthenticated()
		{
			var user = this.TryGetPrincipal(HttpContext.Current);
			return user != null;
		}

		/// <summary>
		/// Authenticates the request by reading the authentication cookie. If the authentication
		/// is successful current request's IPrincipal will be set.
		/// </summary>
		/// <exception cref="NullReferenceException">Thrown if the authentication cookie doesn't have 
		/// a valid FormsAuthenticationTicket.</exception>
		/// <remarks>This method reads the authentication cookie and attempts to retrieve serialized 
		/// IPrincipal from it. The retrieved IPrincipal is set as IPrincipal for the current request.
		/// It is sufficient to call this method just once per single request.</remarks>
		public void AuthenticateRequest()
		{
			var user = HttpContext.Current.User = this.TryGetPrincipal(HttpContext.Current);

			if (user == null)
			{
				// This exeption occurs when a previous auth cookie was encrypted using a different 
				// machine key. This means the machine key has changed and the cookie can no longer 
				// be decrypted, so we simply perform the logout.
				this.Logout();
			}
		}

		/// <summary>
		/// Validates the given credentials.
		/// </summary>
		/// <param name="credentials">TCredentials instance.</param>
		/// <returns>True if credentials are valid, false otherwise.</returns>
		public abstract bool ValidateCredentials(TCredentials credentials);

		private TPrincipal TryGetPrincipal(HttpContext context)
		{
			HttpCookie authCookie = context.Request.Cookies[this.AuthenticationCookieName];

			if (authCookie == null || string.IsNullOrWhiteSpace(authCookie.Value))
			{
				return null;
			}

			try
			{
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

				if (authTicket == null)
				{
					return null;
				}

				return JsonConvert.DeserializeObject<TPrincipal>(authTicket.UserData);
			}
			catch (CryptographicException)
			{
				return null;
			}
		}

		/// <summary>
		/// Retrieves current user's TPrinciple.
		/// </summary>
		/// <returns>TPrinciple instance or null if user is not logged in.</returns>
		public TPrincipal GetPrincipal()
		{
			if (!this.IsRequestAuthenticated())
			{
				return null;
			}

			return HttpContext.Current.User as TPrincipal;
		}

		/// <summary>
		/// Retrieves current user's TPrinciple.
		/// </summary>
		/// <param name="credentials">TCredentials instance.</param>
		/// <returns>TPrinciple instance or null if user is not authenticated.</returns>
		protected abstract TPrincipal GetPrincipal(TCredentials credentials);
	}
}