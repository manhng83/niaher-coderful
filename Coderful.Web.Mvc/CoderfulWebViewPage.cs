namespace Coderful.Web.Mvc
{
	public abstract class CoderfulWebViewPage<TModel, TViewBag> : System.Web.Mvc.WebViewPage<TModel>
		where TViewBag : class
	{
		protected readonly TViewBag StrongViewBag;

		protected CoderfulWebViewPage(IViewBagFactory<TViewBag> viewBagFactory)
		{
			this.StrongViewBag = viewBagFactory.Create(this.ViewBag);
		}
	}
}
