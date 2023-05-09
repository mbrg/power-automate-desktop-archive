using System;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{

	public class AADJoinHostedMachineRequest
	{

		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004B39 File Offset: 0x00002D39
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00004B41 File Offset: 0x00002D41
		public Guid MachineId { get; set; }


		// (get) Token: 0x0600008C RID: 140 RVA: 0x00004B4A File Offset: 0x00002D4A
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00004B52 File Offset: 0x00002D52
		public string PreprovisionBlob { get; set; }


		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004B5B File Offset: 0x00002D5B
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00004B63 File Offset: 0x00002D63
		public string MachineResourceId { get; set; }
	}
}
