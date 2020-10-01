namespace Commerce.Amazon.Domain.Models.Response.Base
{

	public abstract class ResponseBase
	{
		public bool Success { get; set; }
		public string Message { get; set; }

		public TransactionStatus TransactionStatus { get; set; }
		public void MarkAsSuccess()
		{
			MarkAsSuccess(string.Empty, ErrorType.NotError);
		}

		public void MarkAsSuccess(string message)
		{
			MarkAsSuccess(message, ErrorType.NotError);
		}

		public void MarkAsExternalErrorr(string message)
		{
			MarkAsError(message, ErrorType.ExternalError);
		}

		public void MarkAsBusinessError(string message)
		{
			MarkAsError(message, ErrorType.BusinessError);
		}

		public void MarkAsProgrammerError(string message)
		{
			MarkAsError(message, ErrorType.ProgrammerError);
		}

		public void MarkAsError(string message, ErrorType errorType)
		{
			TransactionStatus = new TransactionStatus()
			{
				ErrorType = errorType,
				Message = message,
				TransactionType = TransactionType.Error
			};
		}

		public void MarkAsSuccess(string message, ErrorType errorType)
		{
			TransactionStatus = new TransactionStatus()
			{
				ErrorType = errorType,
				Message = message,
				TransactionType = TransactionType.Success
			};
		}
	}
}
