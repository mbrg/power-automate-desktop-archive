using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PAD.MachineRegistration.Silent.AADJoin
{

	public sealed class NativeMethods
	{

		public static bool IsAadrtAvailable()
		{
			return File.Exists(NativeMethods.AADRT_DLL_PATH);
		}


		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetDllDirectory(string lpPathName);


		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadGetDeviceJoinState")]
		internal static extern int AadGetDeviceJoinState(ushort version, out IntPtr pState);


		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadFreeMemory")]
		internal static extern void AadFreeMemory(IntPtr p);


		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadGetDevicePreprovisioningBlob")]
		internal static extern int AadGetDevicePreprovisioningBlob(string tenantId, string correlationId, out IntPtr pBlob, out IntPtr pJoinCookie);


		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadJoinPreprovisionedDevice")]
		internal static extern int AadJoinPreprovisionedDevice(string joinCookie, string joinChallenge, string tenantId, string correlationId, NativeMethods.AADRT_JOIN_FLAGS joinFlags);


		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadUnjoinDevice")]
		internal static extern int AadUnjoinDevice();


		[DllImport("aadrt.dll", CharSet = CharSet.Auto, EntryPoint = "aadMaintenanceTasks")]
		internal static extern int AadMaintenanceTasks();


		private const string WindowsDefenderFolderName = "Windows Defender Advanced Threat Protection";


		private const string AADRT_DLL_NAME = "aadrt.dll";


		public static readonly string AADRT_DLL_FOLDER = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Windows Defender Advanced Threat Protection");


		public static readonly string AADRT_DLL_PATH = Path.Combine(NativeMethods.AADRT_DLL_FOLDER, "aadrt.dll");


		public enum AADRT_JOIN_FLAGS
		{

			AADRT_JF_NONE,

			AADRT_JF_NO_MEMBERSHIP_CHANGES
		}


		public struct AadDeviceJoinState
		{

			public IntPtr tenantId;


			public IntPtr deviceId;


			public IntPtr certThumbprint;
		}
	}
}
