namespace Coderful.Layouts.Logic.View
{
	using Coderful.Web.Mvc;

	public abstract class MyWebViewPage<TModel> : CoderfulWebViewPage<TModel, StrongViewBag>
	{
		protected MyWebViewPage()
			: base(new StrideViewBagFactory())
		{
		}

		private class StrideViewBagFactory : IViewBagFactory<StrongViewBag>
		{
			public StrongViewBag Create(dynamic viewBag)
			{
				return new StrongViewBag(viewBag);
			}
		}
	}
}
