namespace Coderful.Web.Authentication
{
	using System;
	using System.Net.Http;
	using System.Net.Http.Formatting;
	using System.Threading.Tasks;

	/// <summary>
	/// Utility class that helps work with Mozilla Persona.
	/// </summary>
	public class PersonaAssertionVerifier
	{
		/// <summary>
		/// Verifies assertion and returns associated email address.
		/// </summary>
		/// <param name="assertion">Assertion string.</param>
		/// <param name="requestUri">Url from which the authentication request is coming.</param>
		/// <returns>Email address associated with the assertion. In case the assertion
		/// is invalid this method will return null.</returns>
		public static async Task<string> GetEmail(string assertion, Uri requestUri)
		{
			var personaResponse = await GetAssertionDetails(assertion, requestUri);

			if (personaResponse.Status.Equals("okay", StringComparison.OrdinalIgnoreCase))
			{
				return personaResponse.Email;
			}

			return null;
		}

		/// <summary>
		/// Verifies assertion.
		/// </summary>
		/// <param name="assertion">Assertion string.</param>
		/// <param name="requestUri">Url from which the authentication request is coming.</param>
		/// <returns>Task with PersonaResponse instance.</returns>
		/// <exception cref="NullReferenceException">In case assertion fails for some unexpected reason.</exception>
		public static async Task<PersonaResponse> GetAssertionDetails(string assertion, Uri requestUri)
		{
			var audience = GetAudience(requestUri);

			using (var client = new HttpClient())
			{
				var verifyAssertionAudience = string.Format("https://verifier.login.persona.org/verify?assertion={0}&audience={1}", assertion, audience);
				var response = await client.PostAsync(verifyAssertionAudience, (object)null, new JsonMediaTypeFormatter());

				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsAsync<PersonaResponse>();
				}

				return null;
			}
		}

		private static string GetAudience(Uri requestUri)
		{
			return requestUri.Scheme + "://" + requestUri.Authority;
		}

		/// <summary>
		/// Encapsulates the response received from Persona. For more information see
		/// https://developer.mozilla.org/en/Persona/Remote_Verification_API.
		/// </summary>
		public class PersonaResponse
		{
			/// <summary>
			/// Status of the assertion. The value can be either "okay" or "failure".
			/// </summary>
			public string Status { get; set; }

			/// <summary>
			/// The address contained in the assertion, for the intended person being logged in.
			/// </summary>
			public string Email { get; set; }

			/// <summary>
			/// The audience value contained in the assertion. Expected to be your own website URL.
			/// </summary>
			public string Audience { get; set; }

			/// <summary>
			/// The date the assertion expires.
			/// </summary>
			public long Expires { get; set; }

			/// <summary>
			/// The hostname of the identity provider that issued the assertion.
			/// </summary>
			public string Issuer { get; set; }

			/// <summary>
			/// A string explaining why verification failed.
			/// </summary>
			public string Reason { get; set; }
		}
	}
}