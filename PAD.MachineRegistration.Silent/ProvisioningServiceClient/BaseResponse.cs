using System;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{

	public class BaseResponse<T>
	{



		public T Value { get; set; }




		public string ContinuationToken { get; set; }




		public ApiError Error { get; set; }
	}
}
