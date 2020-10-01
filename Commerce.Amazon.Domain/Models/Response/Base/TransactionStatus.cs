namespace Commerce.Amazon.Domain.Models.Response.Base
{
	public class TransactionStatus
	{
		public TransactionType TransactionType { get; set; }
		public ErrorType ErrorType { get; set; }
		public string Message { get; set; }
		public int ErrorCode { get; set; }
		public decimal Duration { get; set; }
	}
}
