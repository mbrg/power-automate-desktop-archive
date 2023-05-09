using System;

namespace PAD.MachineRegistration.Silent.AADJoin.Entities
{
	// Token: 0x02000022 RID: 34
	public class NativeAPIException : Exception
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000051A6 File Offset: 0x000033A6
		public NativeAPIException()
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x000051AE File Offset: 0x000033AE
		public NativeAPIException(string code, string message)
			: base(string.Concat(new string[] { "message: ", message, ", code: ", code, "." }))
		{
			this.ErrorCode = code;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000051E8 File Offset: 0x000033E8
		public NativeAPIException(string message)
			: base(message)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000051F1 File Offset: 0x000033F1
		public NativeAPIException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000051FB File Offset: 0x000033FB
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00005203 File Offset: 0x00003403
		public string ErrorCode { get; set; }
	}
}
