namespace Coderful.Layouts.Models
{
	using System.Collections.Generic;
	using Coderful.Web;

	/// <summary>
	/// Encapsulates details for layout.
	/// </summary>
	public class LayoutDetails
	{
		public LayoutDetails()
		{
			this.Title = string.Empty;
			this.Breadcrumbs = new List<Link>();
		}

		public string Title { get; set; }
		public List<Link> Breadcrumbs { get; set; }
		public List<Link> Tabs { get; set; }
		public int CurrentTabIndex { get; set; }
	}
}