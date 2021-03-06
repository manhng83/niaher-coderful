namespace Coderful.Permissions
{
	using System.Collections.Generic;

	public interface IPermissionManager<TUserAction, in TUser, in TContext>
	{
		/// <summary>
		/// Checks if user has permission to perform a certain action.
		/// </summary>
		/// <param name="userAction">Action which is to be performed.</param>
		/// <param name="user">User for whom to check the permissions.</param>
		/// <param name="context">Context against which to check the permissions.</param>
		/// <returns>True if the user role(s) give(s) him the permission to perform an action.</returns>
		bool CanDo(TUserAction userAction, TUser user, TContext context);

		/// <summary>
		/// Makes sure that the user has permission to perform a certain action.
		/// If user doesn't have the required permission, then an exception is thrown.
		/// </summary>
		/// <param name="userAction">Action which is to be performed.</param>
		/// <param name="user">User for whom to check the permissions.</param>
		/// <param name="context">Context against which to check the permissions.</param>
		void EnforceCanDo(TUserAction userAction, TUser user, TContext context);

		/// <summary>
		/// Gets a list of allowed user actions for the specified user.
		/// </summary>
		/// <param name="user">User for whom to check the permissions.</param>
		/// <param name="context">Context against which to check the permissions.</param>
		/// <returns>List of allowed user actions.</returns>
		IEnumerable<TUserAction> GetAllowedUserActions(TUser user, TContext context);
	}
}