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




		public string ErrorCode { get; set; }
	}
}
