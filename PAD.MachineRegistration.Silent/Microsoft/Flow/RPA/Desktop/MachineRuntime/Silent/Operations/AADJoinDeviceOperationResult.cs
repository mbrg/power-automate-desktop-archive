using System;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class AADJoinDeviceOperationResult
	{

		// (get) Token: 0x0600004B RID: 75 RVA: 0x00003BBD File Offset: 0x00001DBD
		// (set) Token: 0x0600004C RID: 76 RVA: 0x00003BC5 File Offset: 0x00001DC5
		public bool IsSuccess { get; set; }


		// (get) Token: 0x0600004D RID: 77 RVA: 0x00003BCE File Offset: 0x00001DCE
		// (set) Token: 0x0600004E RID: 78 RVA: 0x00003BD6 File Offset: 0x00001DD6
		public string CorrelationId { get; set; }


		// (get) Token: 0x0600004F RID: 79 RVA: 0x00003BDF File Offset: 0x00001DDF
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003BE7 File Offset: 0x00001DE7
		public string ErrorCode { get; set; }


		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003BF0 File Offset: 0x00001DF0
		// (set) Token: 0x06000052 RID: 82 RVA: 0x00003BF8 File Offset: 0x00001DF8
		public string Message { get; set; }


		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003C01 File Offset: 0x00001E01
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003C09 File Offset: 0x00001E09
		public AADDeviceJoinInfo DeviceState { get; set; }
	}
}
