using System;
using Microsoft.Flow.RPAPAD.Common.Instrumentation;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x0200000D RID: 13
	public class AADJoinDeviceOperationException : Exception
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003B3A File Offset: 0x00001D3A
		public string ErrorCode { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003B42 File Offset: 0x00001D42
		public AADDeviceJoinInfo DeviceJoinInfo { get; }

		// Token: 0x06000049 RID: 73 RVA: 0x00003B4A File Offset: 0x00001D4A
		public AADJoinDeviceOperationException(AADJoinDeviceOperationCode errorCode, string message, AADDeviceJoinInfo deviceJoinInfo = null, Exception innerException = null)
			: base(message, innerException)
		{
			this.ErrorCode = errorCode.ToString();
			this.DeviceJoinInfo = deviceJoinInfo;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003B70 File Offset: 0x00001D70
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
