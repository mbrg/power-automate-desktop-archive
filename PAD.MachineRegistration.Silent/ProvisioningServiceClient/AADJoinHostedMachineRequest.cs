using System;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{

	public class AADJoinHostedMachineRequest
	{



		public Guid MachineId { get; set; }




		public string PreprovisionBlob { get; set; }




		public string MachineResourceId { get; set; }
	}
}
