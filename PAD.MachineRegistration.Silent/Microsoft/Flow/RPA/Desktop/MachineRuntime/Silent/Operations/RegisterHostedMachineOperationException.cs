using System;
using Microsoft.Flow.RPA.RegistrationContract;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class RegisterHostedMachineOperationException : Exception
	{


		public RegistrationError Error { get; }


		public RegisterHostedMachineOperationException(RegistrationError registrationError, string message, Exception innerException = null)
			: base(message, innerException)
		{
			this.Error = registrationError;
		}
	}
}
