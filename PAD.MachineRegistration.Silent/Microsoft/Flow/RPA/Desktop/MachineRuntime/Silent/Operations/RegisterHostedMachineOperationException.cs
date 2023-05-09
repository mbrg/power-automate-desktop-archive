using System;
using Microsoft.Flow.RPA.RegistrationContract;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class RegisterHostedMachineOperationException : Exception
	{

		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003DCE File Offset: 0x00001FCE
		public RegistrationError Error { get; }


		public RegisterHostedMachineOperationException(RegistrationError registrationError, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.Error = registrationError;
		}
	}
}
