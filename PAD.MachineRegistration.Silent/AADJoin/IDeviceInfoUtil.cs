using System;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace PAD.MachineRegistration.Silent.AADJoin
{

	public interface IDeviceInfoUtil
	{

		bool Init();


		bool IsDeviceAADJoined();


		AADDeviceJoinInfo GetDeviceJoinState();


		AADDevicePreprovisioningInfo GetDevicePreprovisioningBlob(string tenantId, string coid);


		bool JoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string coid);


		bool UnjoinDevice();


		void AadMaintenanceTasks();
	}
}
