using System;

namespace Commerce.Amazon.Domain.Entities.CoreBase
{
	public class TResult<T>
	{
		private Exception exception;

		public StatusResponse Status { get; set; }

		public string Message { get; set; }
		public T Result { get; set; }
		public Exception Exception
		{
			get
			{
				return exception;
			}
			set
			{
				//Success = false;
				exception = value;
			}
		}

		public TResult()
		{
		}

		public TResult(T result)
		{
			Result = result;
		}

		public TResult(T result, string message)
		{
			Result = result;
			Message = message;
		}

		public TResult(Exception exception, string message)
		{
			Exception = exception;
			Message = message;
		}
	}
}
