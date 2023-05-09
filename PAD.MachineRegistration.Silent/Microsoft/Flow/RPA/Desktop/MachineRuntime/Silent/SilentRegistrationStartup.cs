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

	public class SilentRegistrationStartup : MachineRegistrationStartup
	{

		public void SetSilentRegistrationCommands(SilentRegistrationCommands silentRegistrationCommands)
		{
			this._silentRegistrationCommands = silentRegistrationCommands;
		}


		protected override void SetupLogging()
		{
			LoggingManager.Initialize(PadLogCategory.SilentRegistration, Component.SilentRegistrationApp, Guid.NewGuid().ToString(), null, null, false);
		}


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


		private SilentRegistrationCommands _silentRegistrationCommands;
	}
}
