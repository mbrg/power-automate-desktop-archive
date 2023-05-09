using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{
	// Token: 0x02000007 RID: 7
	public class SilentRegistrationException : Exception
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003693 File Offset: 0x00001893
		public string ErrorCode { get; }

		// Token: 0x0600003A RID: 58 RVA: 0x0000369B File Offset: 0x0000189B
		public SilentRegistrationException(string errorCode, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.ErrorCode = errorCode;
		}
	}
}
