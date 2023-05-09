using System;

namespace PAD.MachineRegistration.Silent.AADJoin.Entities
{

	public class NativeAPIException : Exception
	{

		public NativeAPIException()
		{
		}


		public NativeAPIException(string code, string message)
			: base(string.Concat(new string[] { "message: ", message, ", code: ", code, "." }))
		{
			this.ErrorCode = code;
		}


		public NativeAPIException(string message)
			: base(message)
		{
		}


		public NativeAPIException(string message, Exception innerException)
			: base(message, innerException)
		{
		}


		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000051FB File Offset: 0x000033FB
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00005203 File Offset: 0x00003403
		public string ErrorCode { get; set; }
	}
}
