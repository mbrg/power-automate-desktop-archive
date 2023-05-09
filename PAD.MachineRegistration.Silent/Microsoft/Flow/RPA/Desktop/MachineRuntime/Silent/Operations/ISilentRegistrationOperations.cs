using System;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Authentication;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public interface ISilentRegistrationOperations
	{

		Task TryLogonAsync(SilentRegistrationAuthenticationFallbackType silentRegistrationAuthenticationFallbackType);


		Task JoinMachineGroupAsync(Guid groupId, SecureString password);


		Task RegisterMachineAsync(string machineName, string machineDescription, bool overrideExistingRegistration);


		Task SetCurrentEnvironmentAsync(SilentRegistrationOperationType operationType, string environmentId = null);


		Task GetStatusAsync(SilentRegistrationFormatType formatType);


		Task GetMachineRegistrationStateAsync();


		Task RegisterHostedMachineAsync(Uri serviceUri, string vmResourceId, SecureString miAuthToken, SecureString groupPassword, string machineName, string machineDescription);


		Task AADJoinDeviceAsync(Uri serviceUri, SecureString miAuthToken, string vmResourceId, string tenantId);


		Task SetServicePlanDetailsAsync();


		Task RecoverMachineAsync();
	}
}
