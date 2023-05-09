using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Data;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Microsoft.Flow.RPAPAD.Common.Instrumentation;
using Microsoft.Flow.RPAPAD.Common.RpcOverNamedPipes.SharedUtility;
using PAD.MachineRegistration.Silent.AADJoin;
using PAD.MachineRegistration.Silent.AADJoin.Entities;
using PAD.MachineRegistration.Silent.ProvisioningServiceClient;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class AADJoinDeviceOperation : IAADJoinDeviceOperation
	{

		public AADJoinDeviceOperation(ILogger<AADJoinDeviceOperation> logger, IDeviceInfoUtil deviceInfoUtil, IProvisioningServiceClient psClient)
		{
			this._logger = logger;
			this._deviceInfoUtil = deviceInfoUtil;
			this._psClient = psClient;
		}


		public async Task<AADJoinDeviceOperationResult> AADJoinDeviceAsync(Uri serviceUri, SecureString miAuthTokenSecure, string vmResourceId, string tenantId, Guid machineId)
		{
			if (!this._deviceInfoUtil.Init())
			{
				throw new AADJoinDeviceOperationException(AADJoinDeviceOperationCode.InitFailed, "The aadrt.dll initialization failed.", null, null);
			}
			AADJoinDeviceOperationResult aadjoinDeviceOperationResult;
			if (this._deviceInfoUtil.IsDeviceAADJoined())
			{
				aadjoinDeviceOperationResult = this.HandleAadJoinedDevice(tenantId);
			}
			else
			{
				AADDevicePreprovisioningInfo devicePreprovisioningInfo = this.GetPreprovisionBlob(tenantId);
				AADJoinHostedMachineResponse aadjoinHostedMachineResponse = await this.PrecreateDeviceAsync(serviceUri, miAuthTokenSecure, vmResourceId, devicePreprovisioningInfo, machineId);
				this.JoinPrecreatedDevice(tenantId, devicePreprovisioningInfo, aadjoinHostedMachineResponse);
				AADDeviceJoinInfo deviceJoinState = this._deviceInfoUtil.GetDeviceJoinState();
				aadjoinDeviceOperationResult = new AADJoinDeviceOperationResult
				{
					IsSuccess = true,
					CorrelationId = RequestCorrelationContext.Current.CorrelationId,
					DeviceState = deviceJoinState
				};
			}
			return aadjoinDeviceOperationResult;
		}


		private void JoinPrecreatedDevice(string tenantId, AADDevicePreprovisioningInfo devicePreprovisioningInfo, AADJoinHostedMachineResponse joinResponse)
		{
			if (!this._deviceInfoUtil.JoinPreprovisionedDevice(devicePreprovisioningInfo.JoinCookie, joinResponse.ChallengeToken, tenantId, RequestCorrelationContext.Current.CorrelationId))
			{
				throw new AADJoinDeviceOperationException(AADJoinDeviceOperationCode.AadrtJoinFailed, "The join operation failed in aadrt.", null, null);
			}
			this._logger.Info("JoinPrecreatedDevice", LogEvent.MachineRegistration.AADJoin, string.Concat(new string[] { "Successfully AAD joined preprovisioned deviceId ", joinResponse.DeviceId, " on tenantId ", tenantId, "." }), -1L, Array.Empty<LogData>());
		}


		private async Task<AADJoinHostedMachineResponse> PrecreateDeviceAsync(Uri serviceUri, SecureString miAuthTokenSecure, string vmResourceId, AADDevicePreprovisioningInfo devicePreprovisioningInfo, Guid machineId)
		{
			AADJoinDeviceOperation.<>c__DisplayClass6_0 CS$<>8__locals1 = new AADJoinDeviceOperation.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.serviceUri = serviceUri;
			CS$<>8__locals1.preCreateRequest = new AADJoinHostedMachineRequest
			{
				MachineId = machineId,
				MachineResourceId = vmResourceId,
				PreprovisionBlob = devicePreprovisioningInfo.Blob
			};
			CS$<>8__locals1.joinResponse = null;
			await miAuthTokenSecure.DecryptSecureStringAndUseResultAsStringAsync(delegate(string miAuthToken)
			{
				AADJoinDeviceOperation.<>c__DisplayClass6_0.<<PrecreateDeviceAsync>b__0>d <<PrecreateDeviceAsync>b__0>d;
				<<PrecreateDeviceAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<PrecreateDeviceAsync>b__0>d.<>4__this = CS$<>8__locals1;
				<<PrecreateDeviceAsync>b__0>d.miAuthToken = miAuthToken;
				<<PrecreateDeviceAsync>b__0>d.<>1__state = -1;
				<<PrecreateDeviceAsync>b__0>d.<>t__builder.Start<AADJoinDeviceOperation.<>c__DisplayClass6_0.<<PrecreateDeviceAsync>b__0>d>(ref <<PrecreateDeviceAsync>b__0>d);
				return <<PrecreateDeviceAsync>b__0>d.<>t__builder.Task;
			});
			if (CS$<>8__locals1.joinResponse == null)
			{
				throw new AADJoinDeviceOperationException(AADJoinDeviceOperationCode.PreprovisionDeviceCallFailure, "The preprovision service response is null.", null, null);
			}
			if (string.IsNullOrEmpty(CS$<>8__locals1.joinResponse.ChallengeToken))
			{
				throw new AADJoinDeviceOperationException(AADJoinDeviceOperationCode.PreprovisionDeviceCallFailure, "The preprovision service response has empty challenge.", null, null);
			}
			this._logger.Info("PrecreateDeviceAsync", LogEvent.MachineRegistration.AADJoin, "Successfully preprovisioned deviceId: " + CS$<>8__locals1.joinResponse.DeviceId, -1L, Array.Empty<LogData>());
			return CS$<>8__locals1.joinResponse;
		}


		private AADDevicePreprovisioningInfo GetPreprovisionBlob(string tenantId)
		{
			AADDevicePreprovisioningInfo devicePreprovisioningBlob = this._deviceInfoUtil.GetDevicePreprovisioningBlob(tenantId, RequestCorrelationContext.Current.CorrelationId);
			if (devicePreprovisioningBlob == null || string.IsNullOrEmpty(devicePreprovisioningBlob.Blob) || string.IsNullOrEmpty(devicePreprovisioningBlob.JoinCookie))
			{
				string text = ((devicePreprovisioningBlob == null) ? "devicePreprovisioningInfo is null" : (string.IsNullOrEmpty(devicePreprovisioningBlob.Blob) ? "Blob is null." : "JoinCookie is null."));
				throw new AADJoinDeviceOperationException(AADJoinDeviceOperationCode.GetDevicePreprovisioningBlobFailure, "Device preprovision info is incorrect, " + text + ".", null, null);
			}
			string text2 = ((devicePreprovisioningBlob.Blob.Length > 20) ? devicePreprovisioningBlob.Blob.Substring(0, 20) : devicePreprovisioningBlob.Blob);
			this._logger.Info("AADJoinDeviceAsync", LogEvent.MachineRegistration.AADJoin, "Successfully got device preprovisioning blob: " + text2, -1L, Array.Empty<LogData>());
			return devicePreprovisioningBlob;
		}


		private AADJoinDeviceOperationResult HandleAadJoinedDevice(string tenantId)
		{
			AADDeviceJoinInfo deviceJoinState = this._deviceInfoUtil.GetDeviceJoinState();
			if (deviceJoinState.TenantId != tenantId)
			{
				throw new AADJoinDeviceOperationException(AADJoinDeviceOperationCode.DeviceAlreadyJoined, string.Concat(new string[] { "The device AAD joined in a different tenant. DeviceId: ", deviceJoinState.DeviceId, ", tenantId Expected: ", tenantId, ", Actual: ", deviceJoinState.TenantId, "." }), deviceJoinState, null);
			}
			this._logger.Info("HandleAadJoinedDevice", LogEvent.MachineRegistration.AADJoin, string.Concat(new string[] { "Device was already successfully registered, deviceId: ", deviceJoinState.DeviceId, ", tenantId: ", deviceJoinState.TenantId, "." }), -1L, Array.Empty<LogData>());
			return new AADJoinDeviceOperationResult
			{
				IsSuccess = true,
				CorrelationId = RequestCorrelationContext.Current.CorrelationId,
				ErrorCode = AADJoinDeviceOperationCode.DeviceAlreadyJoined.ToString(),
				Message = "The device was already AAD joined.",
				DeviceState = deviceJoinState
			};
		}


		private readonly ILogger<AADJoinDeviceOperation> _logger;


		private readonly IDeviceInfoUtil _deviceInfoUtil;


		private readonly IProvisioningServiceClient _psClient;
	}
}
