using System;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace PAD.MachineRegistration.Silent.AADJoin
{
	// Token: 0x0200001E RID: 30
	public interface IDeviceInfoUtil
	{
		// Token: 0x060000A9 RID: 169
		bool Init();

		// Token: 0x060000AA RID: 170
		bool IsDeviceAADJoined();

		// Token: 0x060000AB RID: 171
		AADDeviceJoinInfo GetDeviceJoinState();

		// Token: 0x060000AC RID: 172
		AADDevicePreprovisioningInfo GetDevicePreprovisioningBlob(string tenantId, string coid);

		// Token: 0x060000AD RID: 173
		bool JoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string coid);

		// Token: 0x060000AE RID: 174
		bool UnjoinDevice();

		// Token: 0x060000AF RID: 175
		void AadMaintenanceTasks();
	}
}
