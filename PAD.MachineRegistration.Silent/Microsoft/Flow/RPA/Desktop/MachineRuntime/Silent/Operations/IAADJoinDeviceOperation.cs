using System;
using System.Security;
using System.Threading.Tasks;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public interface IAADJoinDeviceOperation
	{

		Task<AADJoinDeviceOperationResult> AADJoinDeviceAsync(Uri serviceUri, SecureString miAuthToken, string vmResourceId, string tenantId, Guid machineId);
	}
}
