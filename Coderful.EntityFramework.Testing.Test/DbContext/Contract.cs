namespace Coderful.EntityFramework.Testing.Test.DbContext
{
	using System.ComponentModel.DataAnnotations;

	public class Contract
	{
		public Contract(int contractId, int amendmentId)
		{
			this.ContractId = contractId;
			this.AmendmentId = amendmentId;
		}

		[Key]
		public int AmendmentId { get; set; }

		public virtual User ContractHolder { get; set; }

		[Key]
		public int ContractId { get; set; }
	}
}