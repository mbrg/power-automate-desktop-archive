using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.RegistrationContract;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x02000011 RID: 17
	public interface IRegisterHostedMachineOperation
	{
		// Token: 0x06000057 RID: 87
		Task<RegisterHostedMachineResponse> RegisterHostedMachineAsync(Uri serviceUri, string vmResourceId, SecureString miAuthToken, SecureString groupPassword, string machineName, string machineDescription);
	}
}
