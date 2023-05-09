using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public class SilentRegistrationException : Exception
	{

		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003693 File Offset: 0x00001893
		public string ErrorCode { get; }


		public SilentRegistrationException(string errorCode, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.ErrorCode = errorCode;
		}
	}
}
