namespace Coderful.Layouts.Controllers
{
	using System.Web.Mvc;

	public partial class OneController : Controller
	{
		public virtual ActionResult Index()
		{
			return this.View("Index");
		}
	}
}