using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Data;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Newtonsoft.Json;
using PAD.MachineRegistration.Silent.AADJoin.Entities;

namespace PAD.MachineRegistration.Silent.AADJoin
{
	// Token: 0x0200001D RID: 29
	public class DeviceInfoUtil : IDeviceInfoUtil
	{
		// Token: 0x0600009E RID: 158 RVA: 0x00004CA0 File Offset: 0x00002EA0
		public DeviceInfoUtil(ILogger<DeviceInfoUtil> logger)
		{
			this.logger = logger;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004CB0 File Offset: 0x00002EB0
		public bool UnjoinDevice()
		{
			this.logger.Info("UnjoinDevice", LogEvent.MachineRegistration.AADJoin.Start, "UnjoinDevice start.", -1L, Array.Empty<LogData>());
			int num = NativeMethods.AadUnjoinDevice();
			if (num == 0)
			{
				this.logger.Info("UnjoinDevice", LogEvent.MachineRegistration.AADJoin.Success, "UnjoinDevice success.", -1L, Array.Empty<LogData>());
				return true;
			}
			throw new NativeAPIException(this.FormatErrorCode(num), "AADRT AadUnjoinDevice API return error code");
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004D2F File Offset: 0x00002F2F
		public bool IsDeviceAADJoined()
		{
			return this.GetDeviceJoinState() != null;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00004D3C File Offset: 0x00002F3C
		public AADDeviceJoinInfo GetDeviceJoinState()
		{
			this.logger.Info("GetDeviceJoinState", LogEvent.MachineRegistration.AADJoin.Start, "Start Get Device Join State with AAD Runtime.", -1L, Array.Empty<LogData>());
			IntPtr pState = IntPtr.Zero;
			return this.aadrtRetryPolicy<AADDeviceJoinInfo>("GetDeviceJoinState", delegate
			{
				AADDeviceJoinInfo aaddeviceJoinInfo;
				try
				{
					int num = NativeMethods.AadGetDeviceJoinState(1, out pState);
					if (num != 0)
					{
						throw new NativeAPIException(this.FormatErrorCode(num), "AADRT AadGetDeviceJoinState API return error code");
					}
					if (pState == IntPtr.Zero)
					{
						this.logger.Info("GetDeviceJoinState", LogEvent.MachineRegistration.AADJoin.Start, "Join state is null, this device may not AADJ yet", -1L, Array.Empty<LogData>());
						aaddeviceJoinInfo = null;
					}
					else
					{
						NativeMethods.AadDeviceJoinState aadDeviceJoinState = Marshal.PtrToStructure<NativeMethods.AadDeviceJoinState>(pState);
						AADDeviceJoinInfo aaddeviceJoinInfo2 = new AADDeviceJoinInfo
						{
							TenantId = Marshal.PtrToStringAuto(aadDeviceJoinState.tenantId),
							DeviceId = Marshal.PtrToStringAuto(aadDeviceJoinState.deviceId),
							Thumbprint = Marshal.PtrToStringAuto(aadDeviceJoinState.certThumbprint)
						};
						string text = "GetDeviceJoinState success: " + JsonConvert.SerializeObject(aaddeviceJoinInfo2) + ".";
						if (string.IsNullOrEmpty(aaddeviceJoinInfo2.TenantId) || string.IsNullOrEmpty(aaddeviceJoinInfo2.DeviceId) || string.IsNullOrEmpty(aaddeviceJoinInfo2.Thumbprint))
						{
							throw new KeyNotFoundException(text + "empty value is invalid.");
						}
						aaddeviceJoinInfo = aaddeviceJoinInfo2;
					}
				}
				finally
				{
					NativeMethods.AadFreeMemory(pState);
				}
				return aaddeviceJoinInfo;
			}).GetAwaiter().GetResult();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public AADDevicePreprovisioningInfo GetDevicePreprovisioningBlob(string tenantId, string correlationId)
		{
			if (string.IsNullOrEmpty(tenantId))
			{
				throw new ArgumentNullException("tenantId cannot be null");
			}
			this.logger.Info("GetDevicePreprovisioningBlob", LogEvent.MachineRegistration.AADJoin.Start, string.Concat(new string[] { "Start Get Device Preprovision Blob, tenantId: ", tenantId, ", correlationId: ", correlationId, "." }), -1L, Array.Empty<LogData>());
			IntPtr pBlob = IntPtr.Zero;
			IntPtr pJoinCookie = IntPtr.Zero;
			return this.aadrtRetryPolicy<AADDevicePreprovisioningInfo>("GetDevicePreprovisioningBlob", delegate
			{
				AADDevicePreprovisioningInfo aaddevicePreprovisioningInfo2;
				try
				{
					int num = NativeMethods.AadGetDevicePreprovisioningBlob(tenantId, correlationId, out pBlob, out pJoinCookie);
					if (num != 0)
					{
						throw new NativeAPIException(this.FormatErrorCode(num), "AADRT AadGetDevicePreprovisioningBlob API return error code");
					}
					AADDevicePreprovisioningInfo aaddevicePreprovisioningInfo = new AADDevicePreprovisioningInfo();
					aaddevicePreprovisioningInfo.Blob = Marshal.PtrToStringAuto(pBlob);
					aaddevicePreprovisioningInfo.JoinCookie = Marshal.PtrToStringAuto(pJoinCookie);
					this.logger.Info("GetDevicePreprovisioningBlob", LogEvent.MachineRegistration.AADJoin.Success, "Get device preprovision blob succeed", -1L, Array.Empty<LogData>());
					aaddevicePreprovisioningInfo2 = aaddevicePreprovisioningInfo;
				}
				finally
				{
					NativeMethods.AadFreeMemory(pBlob);
					NativeMethods.AadFreeMemory(pJoinCookie);
				}
				return aaddevicePreprovisioningInfo2;
			}).GetAwaiter().GetResult();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004E88 File Offset: 0x00003088
		public bool JoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string correlationId)
		{
			if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(joinCookie) || string.IsNullOrEmpty(joinChallenge))
			{
				throw new ArgumentNullException("JoinPreprovisionedDevice argument cannot be null");
			}
			Func<bool> <>9__0;
			for (int i = 0; i < 7; i++)
			{
				try
				{
					string text = "JoinPreprovisionedDevice";
					Func<bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = delegate
						{
							this.logger.Info("JoinPreprovisionedDevice", LogEvent.MachineRegistration.AADJoin.Start, string.Concat(new string[] { "Start JoinPreprovisionedDevice, tenantId: ", tenantId, ", correlationId: ", correlationId, "." }), -1L, Array.Empty<LogData>());
							int num = this.AadJoinPreprovisionedDevice(joinCookie, joinChallenge, tenantId, correlationId);
							if (num != 0)
							{
								throw new NativeAPIException(this.FormatErrorCode(num), "AADRT AadJoinPreprovisionedDevice API return error code.");
							}
							this.logger.Info("JoinPreprovisionedDevice", LogEvent.MachineRegistration.AADJoin.Success, "JoinPreprovisionedDevice succeed", -1L, Array.Empty<LogData>());
							return true;
						});
					}
					return this.aadrtRetryPolicy<bool>(text, func).GetAwaiter().GetResult();
				}
				catch (NativeAPIException ex) when (ex.ErrorCode == "0x801C03F3")
				{
					if (i == 6)
					{
						this.logger.Error("JoinPreprovisionedDevice", LogEvent.MachineRegistration.AADJoin.Exception, "JoinPreprovisionedDevice execute exception, last attempt failed.", ex, -1L, Array.Empty<LogData>());
						throw;
					}
					TimeSpan timeSpan = TimeSpan.FromSeconds(25.0);
					this.logger.Error("JoinPreprovisionedDevice", LogEvent.MachineRegistration.AADJoin.Exception, string.Format("{0} execute exception, attempt {1} timeSpan: {2}.", "JoinPreprovisionedDevice", i, timeSpan), ex, -1L, Array.Empty<LogData>());
					Task.Delay(timeSpan).GetAwaiter().GetResult();
				}
			}
			throw new Exception("Unreachable code");
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005024 File Offset: 0x00003224
		public void AadMaintenanceTasks()
		{
			NativeMethods.AadMaintenanceTasks();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000502C File Offset: 0x0000322C
		protected virtual int AadJoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string correlationId)
		{
			return NativeMethods.AadJoinPreprovisionedDevice(joinCookie, joinChallenge, tenantId, correlationId, NativeMethods.AADRT_JOIN_FLAGS.AADRT_JF_NONE);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00005039 File Offset: 0x00003239
		private string FormatErrorCode(int code)
		{
			return "0x" + code.ToString("X");
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005054 File Offset: 0x00003254
		private async Task<T> aadrtRetryPolicy<T>(string name, Func<T> action)
		{
			int num;
			for (int retryAttempt = 0; retryAttempt < 3; retryAttempt = num + 1)
			{
				num = 0;
				try
				{
					return action();
				}
				catch (NativeAPIException obj)
				{
					num = 1;
				}
				if (num == 1)
				{
					object obj;
					NativeAPIException ex = (NativeAPIException)obj;
					TimeSpan timeSpan = TimeSpan.FromSeconds(Math.Pow(1.5, (double)retryAttempt));
					if (retryAttempt == 2)
					{
						this.logger.Error(name, LogEvent.MachineRegistration.AADJoin.Exception, string.Format("Last attempt {0} failed.", retryAttempt), ex, -1L, Array.Empty<LogData>());
						Exception ex2 = obj as Exception;
						if (ex2 == null)
						{
							throw obj;
						}
						ExceptionDispatchInfo.Capture(ex2).Throw();
					}
					this.logger.Error(name, LogEvent.MachineRegistration.AADJoin.Exception, string.Format("Attempt {0} failed. Retry after {1}.", retryAttempt, timeSpan), ex, -1L, Array.Empty<LogData>());
					await Task.Delay(timeSpan);
				}
				num = retryAttempt;
			}
			throw new Exception("Unreachable code.");
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000050A8 File Offset: 0x000032A8
		public bool Init()
		{
			if (!NativeMethods.IsAadrtAvailable())
			{
				this.logger.Error("Init", LogEvent.MachineRegistration.AADJoin.Failure, "Could not init the aadrt.dll, the DLL is missing.", null, -1L, Array.Empty<LogData>());
				return false;
			}
			if (NativeMethods.SetDllDirectory(NativeMethods.AADRT_DLL_FOLDER))
			{
				this.AadMaintenanceTasks();
				this.logger.Info("Init", LogEvent.MachineRegistration.AADJoin.Failure, "Found and initialized aadrt.dll", -1L, Array.Empty<LogData>());
				return true;
			}
			this.logger.Error("Init", LogEvent.MachineRegistration.AADJoin.Failure, "Could not set the aadrt.dll directory.", null, -1L, Array.Empty<LogData>());
			return false;
		}

		// Token: 0x040000A1 RID: 161
		private const int MaxRetryCount = 7;

		// Token: 0x040000A2 RID: 162
		private const int MaxCommonRetryCount = 3;

		// Token: 0x040000A3 RID: 163
		private const double CommonRetryBaseTime = 1.5;

		// Token: 0x040000A4 RID: 164
		private readonly ILogger<DeviceInfoUtil> logger;
	}
}
