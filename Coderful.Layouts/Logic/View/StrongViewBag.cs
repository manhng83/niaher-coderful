namespace Coderful.Layouts.Logic.View
{
	using Coderful.Layouts.Models;

	public class StrongViewBag
	{
		private readonly dynamic viewBag;

		public StrongViewBag(dynamic viewBag)
		{
			this.viewBag = viewBag;
		}

		public LayoutDetails LayoutDetails
		{
			get
			{
				return this.viewBag.LayoutDetails;
			}

			set
			{
				this.viewBag.LayoutDetails = value;
			}
		}
	}
}