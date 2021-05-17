namespace Coderful.Layouts
{
	using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
			bundles.Add(new StyleBundle("~/css/one").Include("~/Styles/One.css"));
			bundles.Add(new StyleBundle("~/css/two").Include("~/Styles/Two.css"));

			bundles.Add(new StyleBundle("~/css/bootstrap", "//netdna.bootstrapcdn.com/bootstrap/3.0.0-rc1/css/bootstrap.min.css").Include(
				"~/Styles/Bootstrap/css/bootstrap.css"));

			bundles.Add(new StyleBundle("~/css/fa").Include(
				"~/Styles/FontAwesome/css/font-awesome.css"));
			
			bundles.Add(new StyleBundle("~/js/libs/html5shiv", "//oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js").Include(
				"~/Scripts/Libs/html5shiv.js"));

			bundles.Add(new StyleBundle("~/js/libs/respond", "//oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js").Include(
				"~/Scripts/Libs/respond.min.js"));
        }
    }
}
