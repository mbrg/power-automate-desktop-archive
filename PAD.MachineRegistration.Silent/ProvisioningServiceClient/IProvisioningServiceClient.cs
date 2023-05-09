using System;
using System.Threading;
using System.Threading.Tasks;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{
	// Token: 0x0200001B RID: 27
	public interface IProvisioningServiceClient
	{
		// Token: 0x06000096 RID: 150
		Task<AADJoinHostedMachineResponse> AADJoinHostedMachineRequestAsync(Uri serviceUri, string authToken, AADJoinHostedMachineRequest request, CancellationToken cancellationToken = default(CancellationToken));
	}
}
