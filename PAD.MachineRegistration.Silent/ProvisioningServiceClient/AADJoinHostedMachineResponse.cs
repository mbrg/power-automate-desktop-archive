using System;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{

	public class AADJoinHostedMachineResponse
	{

		// (get) Token: 0x06000091 RID: 145 RVA: 0x00004B74 File Offset: 0x00002D74
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00004B7C File Offset: 0x00002D7C
		public string ChallengeToken { get; set; }


		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004B85 File Offset: 0x00002D85
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00004B8D File Offset: 0x00002D8D
		public string DeviceId { get; set; }
	}
}
