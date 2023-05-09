using System;
using Microsoft.Flow.RPAPAD.Common.Instrumentation;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class AADJoinDeviceOperationException : Exception
	{

		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003B3A File Offset: 0x00001D3A
		public string ErrorCode { get; }


		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003B42 File Offset: 0x00001D42
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
