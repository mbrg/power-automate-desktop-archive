using System;
using Microsoft.Flow.RPA.RegistrationContract;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x02000015 RID: 21
	public class RegisterHostedMachineOperationException : Exception
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003DCE File Offset: 0x00001FCE
		public RegistrationError Error { get; }

		// Token: 0x06000068 RID: 104 RVA: 0x00003DD6 File Offset: 0x00001FD6
		public RegisterHostedMachineOperationException(RegistrationError registrationError, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.Error = registrationError;
		}
	}
}
