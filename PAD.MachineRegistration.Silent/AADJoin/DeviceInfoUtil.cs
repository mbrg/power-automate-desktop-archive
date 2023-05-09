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

	public class DeviceInfoUtil : IDeviceInfoUtil
	{

		public DeviceInfoUtil(ILogger<DeviceInfoUtil> logger)
		{
			this.logger = logger;
		}


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


		public bool IsDeviceAADJoined()
		{
			return this.GetDeviceJoinState() != null;
		}


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


		public void AadMaintenanceTasks()
		{
			NativeMethods.AadMaintenanceTasks();
		}


		protected virtual int AadJoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string correlationId)
		{
			return NativeMethods.AadJoinPreprovisionedDevice(joinCookie, joinChallenge, tenantId, correlationId, NativeMethods.AADRT_JOIN_FLAGS.AADRT_JF_NONE);
		}


		private string FormatErrorCode(int code)
		{
			return "0x" + code.ToString("X");
		}


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


		private const int MaxRetryCount = 7;


		private const int MaxCommonRetryCount = 3;


		private const double CommonRetryBaseTime = 1.5;


		private readonly ILogger<DeviceInfoUtil> logger;
	}
}
