namespace Coderful.Web.Mvc
{
	using System.Data.Entity;
	using Breeze.ContextProvider.EF6;

	public abstract class BreezeController<TDbContext, TContextProvider> : BreezeController<TDbContext>
		where TDbContext : DbContext, new()
		where TContextProvider : EFContextProvider<TDbContext>, new()
	{
		protected BreezeController()
			: this(new TContextProvider())
		{
		}

		protected BreezeController(TContextProvider contextProvider)
			: base(contextProvider)
		{
		}
	}
}
