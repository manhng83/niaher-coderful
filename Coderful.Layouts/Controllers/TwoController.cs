namespace Coderful.Layouts.Controllers
{
	using System.Web.Mvc;

	public partial class TwoController : Controller
	{
		public virtual ActionResult Index(int? tab)
		{
			this.ViewBag.Tab = tab == null ? 1 : tab.Value;
			return this.View("Index");
		}
	}
}