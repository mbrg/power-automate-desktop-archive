using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public enum AADJoinDeviceOperationCode
	{

		Success = 1,

		InitFailed,

		DeviceAlreadyJoined,

		GetDevicePreprovisioningBlobFailure,

		PreprovisionDeviceCallFailure,

		AadrtJoinFailed,

		MachineNotRegistered,

		InvalidMachineState
	}
}
