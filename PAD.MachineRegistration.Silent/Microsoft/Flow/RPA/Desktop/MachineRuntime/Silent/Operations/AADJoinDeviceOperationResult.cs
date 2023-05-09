using System;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class AADJoinDeviceOperationResult
	{



		public bool IsSuccess { get; set; }




		public string CorrelationId { get; set; }




		public string ErrorCode { get; set; }




		public string Message { get; set; }




		public AADDeviceJoinInfo DeviceState { get; set; }
	}
}
