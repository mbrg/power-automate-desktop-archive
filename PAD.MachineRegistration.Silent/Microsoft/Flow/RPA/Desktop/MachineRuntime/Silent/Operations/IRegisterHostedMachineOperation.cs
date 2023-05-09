using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.RegistrationContract;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public interface IRegisterHostedMachineOperation
	{

		Task<RegisterHostedMachineResponse> RegisterHostedMachineAsync(Uri serviceUri, string vmResourceId, SecureString miAuthToken, SecureString groupPassword, string machineName, string machineDescription);
	}
}
