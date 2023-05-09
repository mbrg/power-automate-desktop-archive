using System;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Microsoft.Flow.RPA.Desktop.UIFlowService;
using Microsoft.Flow.RPAPAD.Common.Logging;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.UIFlowService
{

	public class SilentRegistrationUIFlowServicePipe : UIFlowServicePipeBase<SilentRegistrationUIFlowServicePipe>
	{

		public SilentRegistrationUIFlowServicePipe(ILogger<SilentRegistrationUIFlowServicePipe> logger)
			: base(logger, "NamedPipe_SilentRegistrationApp_Service", LogCategory.SilentRegistration)
		{
		}


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
