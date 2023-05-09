using System;
using System.Threading;
using System.Threading.Tasks;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{

	public interface IProvisioningServiceClient
	{

		Task<AADJoinHostedMachineResponse> AADJoinHostedMachineRequestAsync(Uri serviceUri, string authToken, AADJoinHostedMachineRequest request, CancellationToken cancellationToken = default(CancellationToken));
	}
}
