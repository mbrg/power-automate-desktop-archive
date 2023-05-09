using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public class SilentRegistrationException : Exception
	{


		public string ErrorCode { get; }


		public SilentRegistrationException(string errorCode, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.ErrorCode = errorCode;
		}
	}
}
