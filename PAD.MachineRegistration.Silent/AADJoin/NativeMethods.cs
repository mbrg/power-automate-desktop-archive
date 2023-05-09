using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PAD.MachineRegistration.Silent.AADJoin
{
	// Token: 0x0200001F RID: 31
	public sealed class NativeMethods
	{
		// Token: 0x060000B0 RID: 176 RVA: 0x00005156 File Offset: 0x00003356
		public static bool IsAadrtAvailable()
		{
			return File.Exists(NativeMethods.AADRT_DLL_PATH);
		}

		// Token: 0x060000B1 RID: 177
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetDllDirectory(string lpPathName);

		// Token: 0x060000B2 RID: 178
		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadGetDeviceJoinState")]
		internal static extern int AadGetDeviceJoinState(ushort version, out IntPtr pState);

		// Token: 0x060000B3 RID: 179
		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadFreeMemory")]
		internal static extern void AadFreeMemory(IntPtr p);

		// Token: 0x060000B4 RID: 180
		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadGetDevicePreprovisioningBlob")]
		internal static extern int AadGetDevicePreprovisioningBlob(string tenantId, string correlationId, out IntPtr pBlob, out IntPtr pJoinCookie);

		// Token: 0x060000B5 RID: 181
		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadJoinPreprovisionedDevice")]
		internal static extern int AadJoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string correlationId, NativeMethods.AADRT_JOIN_FLAGS joinFlags);

		// Token: 0x060000B6 RID: 182
		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadUnjoinDevice")]
		internal static extern int AadUnjoinDevice();

		// Token: 0x060000B7 RID: 183
		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadMaintenanceTasks")]
		internal static extern int AadMaintenanceTasks();

		// Token: 0x040000A5 RID: 165
		private const string WindowsDefenderFolderName = "Windows Defender Advanced Threat Protection";

		// Token: 0x040000A6 RID: 166
		private const string AADRT_DLL_NAME = "aadrt.dll";

		// Token: 0x040000A7 RID: 167
		public static readonly string AADRT_DLL_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Windows Defender Advanced Threat Protection");

		// Token: 0x040000A8 RID: 168
		public static readonly string AADRT_DLL_PATH = Path.Combine(NativeMethods.AADRT_DLL_FOLDER, "aadrt.dll");

		// Token: 0x02000059 RID: 89
		public enum AADRT_JOIN_FLAGS
		{
			// Token: 0x040001C5 RID: 453
			AADRT_JF_NONE,
			// Token: 0x040001C6 RID: 454
			AADRT_JF_NO_MEMBERSHIP_CHANGES
		}

		// Token: 0x0200005A RID: 90
		public struct AadDeviceJoinState
		{
			// Token: 0x040001C7 RID: 455
			public IntPtr tenantId;

			// Token: 0x040001C8 RID: 456
			public IntPtr deviceId;

			// Token: 0x040001C9 RID: 457
			public IntPtr certThumbprint;
		}
	}
}
