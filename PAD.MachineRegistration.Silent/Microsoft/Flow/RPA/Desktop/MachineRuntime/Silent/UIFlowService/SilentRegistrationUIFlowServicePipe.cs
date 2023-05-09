using System;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Microsoft.Flow.RPA.Desktop.UIFlowService;
using Microsoft.Flow.RPAPAD.Common.Logging;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.UIFlowService
{
	// Token: 0x02000009 RID: 9
	public class SilentRegistrationUIFlowServicePipe : UIFlowServicePipeBase<SilentRegistrationUIFlowServicePipe>
	{
		// Token: 0x0600003F RID: 63 RVA: 0x0000375C File Offset: 0x0000195C
		public SilentRegistrationUIFlowServicePipe(ILogger<SilentRegistrationUIFlowServicePipe> logger)
			: base(logger, "NamedPipe_SilentRegistrationApp_Service", LogCategory.SilentRegistration)
		{
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000376C File Offset: 0x0000196C
		public override void InitializePipe()
		{
			try
			{
				base.InitializePipe();
			}
			catch (AggregateException ex) when (ex.InnerException is UnauthorizedAccessException)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.SilentOperationAlreadyInProgress.ToString(), "Cannot execute operation, there is already a silent operation in progress. Please retry.", null);
			}
		}
	}
}
