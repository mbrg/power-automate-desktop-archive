using System;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Application.Machine;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Microsoft.Flow.RPA.Desktop.UIFlowService;
using Microsoft.Flow.RPA.RegistrationContract;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{
	// Token: 0x02000014 RID: 20
	public class RegisterHostedMachineOperation : IRegisterHostedMachineOperation
	{
		// Token: 0x06000065 RID: 101 RVA: 0x00003D42 File Offset: 0x00001F42
		public RegisterHostedMachineOperation(ILogger<RegisterHostedMachineOperation> logger, IUIFlowServicePipe uIFlowServicePipe)
		{
			this._logger = logger;
			this._uIFlowServicePipe = uIFlowServicePipe;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003D58 File Offset: 0x00001F58
		public async Task<RegisterHostedMachineResponse> RegisterHostedMachineAsync(Uri serviceUri, string vmResourceId, SecureString miAuthTokenSecure, SecureString groupPasswordSecure, string machineName, string machineDescription)
		{
			RegisterHostedMachineOperation.<>c__DisplayClass3_0 CS$<>8__locals1 = new RegisterHostedMachineOperation.<>c__DisplayClass3_0();
			CS$<>8__locals1.miAuthTokenSecure = miAuthTokenSecure;
			CS$<>8__locals1.groupPasswordSecure = groupPasswordSecure;
			CS$<>8__locals1.serviceUri = serviceUri;
			CS$<>8__locals1.vmResourceId = vmResourceId;
			CS$<>8__locals1.machineName = machineName;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.machineDescription = machineDescription;
			RegisterHostedMachineResponse registerHostedMachineResponse2;
			try
			{
				RegisterHostedMachineResponse registerHostedMachineResponse = await MachineRegistrationManagerHelpers.ExecuteRegistrationOperationAsync<RegisterHostedMachineResponse, RegisterHostedMachineOperation>(this._logger, delegate
				{
					RegisterHostedMachineOperation.<>c__DisplayClass3_0.<<RegisterHostedMachineAsync>b__0>d <<RegisterHostedMachineAsync>b__0>d;
					<<RegisterHostedMachineAsync>b__0>d.<>t__builder = AsyncTaskMethodBuilder<RegisterHostedMachineResponse>.Create();
					<<RegisterHostedMachineAsync>b__0>d.<>4__this = CS$<>8__locals1;
					<<RegisterHostedMachineAsync>b__0>d.<>1__state = -1;
					<<RegisterHostedMachineAsync>b__0>d.<>t__builder.Start<RegisterHostedMachineOperation.<>c__DisplayClass3_0.<<RegisterHostedMachineAsync>b__0>d>(ref <<RegisterHostedMachineAsync>b__0>d);
					return <<RegisterHostedMachineAsync>b__0>d.<>t__builder.Task;
				}, LogEvent.MachineRegistration.Machine.RegisterHosted, "RegisterHostedMachineAsync");
				if (!registerHostedMachineResponse.IsSuccessful)
				{
					throw new RegisterHostedMachineOperationException(registerHostedMachineResponse.Error, "Registration failed.", null);
				}
				registerHostedMachineResponse2 = registerHostedMachineResponse;
			}
			catch (Exception ex) when (!(ex is RegisterHostedMachineOperationException))
			{
				throw new RegisterHostedMachineOperationException(null, "Registration failed with unhandled exception.", ex);
			}
			return registerHostedMachineResponse2;
		}

		// Token: 0x04000075 RID: 117
		private readonly ILogger<RegisterHostedMachineOperation> _logger;

		// Token: 0x04000076 RID: 118
		private readonly IUIFlowServicePipe _uIFlowServicePipe;
	}
}
