namespace Coderful.Web.Identity
{
	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Services;
	using System.IdentityModel.Tokens;
	using System.Security.Claims;
	using System.Security.Principal;

	public abstract class Authenticator : ClaimsAuthenticationManager
	{
		public void Authenticate(string userName)
		{
			List<Claim> initialClaims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, userName)
			};

			var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(initialClaims, "Forms"));
			this.Authenticate(String.Empty, claimsPrincipal);
		}

		public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
		{
			if (!incomingPrincipal.Identity.IsAuthenticated)
			{
				return base.Authenticate(resourceName, incomingPrincipal);
			}

			var transformedPrincipal = this.GetApplicationPrincipal(incomingPrincipal);

			CreateSession(transformedPrincipal);

			return transformedPrincipal;
		}

		protected abstract ClaimsPrincipal GetApplicationPrincipal(IPrincipal incomingPrincipal);

		private static void CreateSession(ClaimsPrincipal transformedPrincipal)
		{
			var sessionSecurityToken = new SessionSecurityToken(transformedPrincipal, TimeSpan.FromHours(8));
			FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionSecurityToken);
		}
	}
}