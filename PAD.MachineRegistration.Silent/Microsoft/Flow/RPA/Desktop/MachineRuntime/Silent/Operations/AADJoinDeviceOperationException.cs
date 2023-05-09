using System;
using Microsoft.Flow.RPAPAD.Common.Instrumentation;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class AADJoinDeviceOperationException : Exception
	{


		public string ErrorCode { get; }



		public AADDeviceJoinInfo DeviceJoinInfo { get; }


		public AADJoinDeviceOperationException(AADJoinDeviceOperationCode errorCode, string message, AADDeviceJoinInfo deviceJoinInfo = null, Exception innerException = null)
			: base(message, innerException)
		{
			this.ErrorCode = errorCode.ToString();
			this.DeviceJoinInfo = deviceJoinInfo;
		}


		public AADJoinDeviceOperationResult ToErrorResult()
		{
			return new AADJoinDeviceOperationResult
			{
				CorrelationId = RequestCorrelationContext.Current.CorrelationId,
				DeviceState = this.DeviceJoinInfo,
				ErrorCode = this.ErrorCode,
				Message = this.Message,
				IsSuccess = false
			};
		}
	}
}
