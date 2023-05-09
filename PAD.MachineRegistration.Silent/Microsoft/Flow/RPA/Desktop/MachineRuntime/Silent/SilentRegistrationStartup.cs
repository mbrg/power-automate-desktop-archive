using System;
using Autofac;
using Autofac.Builder;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Common;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Authentication;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.UIFlowService;
using Microsoft.Flow.RPA.Desktop.Shared.Logging;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Enums;
using Microsoft.Flow.RPA.Desktop.Shared.Telemetry.Instrumentation.Enums;
using PAD.MachineRegistration.Silent.AADJoin;
using PAD.MachineRegistration.Silent.ProvisioningServiceClient;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{
	// Token: 0x02000008 RID: 8
	public class SilentRegistrationStartup : MachineRegistrationStartup
	{
		// Token: 0x0600003B RID: 59 RVA: 0x000036AC File Offset: 0x000018AC
		public void SetSilentRegistrationCommands(SilentRegistrationCommands silentRegistrationCommands)
		{
			this._silentRegistrationCommands = silentRegistrationCommands;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000036B8 File Offset: 0x000018B8
		protected override void SetupLogging()
		{
			LoggingManager.Initialize(PadLogCategory.SilentRegistration, Component.SilentRegistrationApp, Guid.NewGuid().ToString(), null, null, false);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000036E4 File Offset: 0x000018E4
		protected override void SetupAppSpecificIoC(ContainerBuilder builder)
		{
			builder.RegisterType<SilentRegistrationTokenProvider>().AsImplementedInterfaces<SilentRegistrationTokenProvider, ConcreteReflectionActivatorData>();
			builder.RegisterType<SilentRegistrationUIFlowServicePipe>().AsImplementedInterfaces<SilentRegistrationUIFlowServicePipe, ConcreteReflectionActivatorData>().SingleInstance();
			builder.RegisterType<RegisterHostedMachineOperation>().AsImplementedInterfaces<RegisterHostedMachineOperation, ConcreteReflectionActivatorData>();
			builder.RegisterType<DeviceInfoUtil>().AsImplementedInterfaces<DeviceInfoUtil, ConcreteReflectionActivatorData>();
			builder.RegisterType<AADJoinDeviceOperation>().AsImplementedInterfaces<AADJoinDeviceOperation, ConcreteReflectionActivatorData>();
			builder.RegisterType<ProvisioningServiceClient>().AsImplementedInterfaces<ProvisioningServiceClient, ConcreteReflectionActivatorData>().SingleInstance();
			builder.RegisterType<SilentRegistrationOperations>().AsImplementedInterfaces<SilentRegistrationOperations, ConcreteReflectionActivatorData>().SingleInstance();
		}

		// Token: 0x04000053 RID: 83
		private SilentRegistrationCommands _silentRegistrationCommands;
	}
}
