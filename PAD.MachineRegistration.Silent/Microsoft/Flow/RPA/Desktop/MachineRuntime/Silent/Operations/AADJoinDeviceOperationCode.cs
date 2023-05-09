using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x0200000E RID: 14
	public enum AADJoinDeviceOperationCode
	{
		// Token: 0x04000068 RID: 104
		Success = 1,
		// Token: 0x04000069 RID: 105
		InitFailed,
		// Token: 0x0400006A RID: 106
		DeviceAlreadyJoined,
		// Token: 0x0400006B RID: 107
		GetDevicePreprovisioningBlobFailure,
		// Token: 0x0400006C RID: 108
		PreprovisionDeviceCallFailure,
		// Token: 0x0400006D RID: 109
		AadrtJoinFailed,
		// Token: 0x0400006E RID: 110
		MachineNotRegistered,
		// Token: 0x0400006F RID: 111
		InvalidMachineState
	}
}
