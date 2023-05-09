using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Authentication;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x02000012 RID: 18
	public interface ISilentRegistrationOperations
	{
		// Token: 0x06000058 RID: 88
		Task TryLogonAsync(SilentRegistrationAuthenticationFallbackType silentRegistrationAuthenticationFallbackType);

		// Token: 0x06000059 RID: 89
		Task JoinMachineGroupAsync(Guid groupId, SecureString password);

		// Token: 0x0600005A RID: 90
		Task RegisterMachineAsync(string machineName, string machineDescription, bool overrideExistingRegistration);

		// Token: 0x0600005B RID: 91
		Task SetCurrentEnvironmentAsync(SilentRegistrationOperationType operationType, string environmentId = null);

		// Token: 0x0600005C RID: 92
		Task GetStatusAsync(SilentRegistrationFormatType formatType);

		// Token: 0x0600005D RID: 93
		Task GetMachineRegistrationStateAsync();

		// Token: 0x0600005E RID: 94
		Task RegisterHostedMachineAsync(Uri serviceUri, string vmResourceId, SecureString miAuthToken, SecureString groupPassword, string machineName, string machineDescription);

		// Token: 0x0600005F RID: 95
		Task AADJoinDeviceAsync(Uri serviceUri, SecureString miAuthToken, string vmResourceId, string tenantId);

		// Token: 0x06000060 RID: 96
		Task SetServicePlanDetailsAsync();

		// Token: 0x06000061 RID: 97
		Task RecoverMachineAsync();
	}
}
