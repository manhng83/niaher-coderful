namespace Coderful.Web.Mvc
{
	public interface IViewBagFactory<out TViewBag>
		where TViewBag : class
	{
		TViewBag Create(dynamic viewBag);
	}
}