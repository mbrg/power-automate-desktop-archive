using System;
using System.Security;
using System.Threading.Tasks;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x02000010 RID: 16
	public interface IAADJoinDeviceOperation
	{
		// Token: 0x06000056 RID: 86
		Task<AADJoinDeviceOperationResult> AADJoinDeviceAsync(Uri serviceUri, SecureString miAuthToken, string vmResourceId, string tenantId, Guid machineId);
	}
}
