using System;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using Microsoft.Flow.RPA.Desktop.Common.Services.Registry;
using Microsoft.Flow.RPA.Desktop.Console.Application.Services;
using Microsoft.Flow.RPA.Desktop.Console.Core.ApplicationRoot;
using Microsoft.Flow.RPA.Desktop.Console.Core.UserAccount;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;
using Microsoft.Flow.RPA.Desktop.Shared.Clients.TenantDiscovery.Entities;
using Microsoft.Flow.RPA.Desktop.Shared.Common.Enums;
using Microsoft.Flow.RPA.Desktop.Shared.Localization.Properties;
using Microsoft.Flow.RPA.Desktop.Shared.Logging;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Data;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Microsoft.Flow.RPA.Desktop.Shared.MsAccountAuthenticator;
using Microsoft.Flow.RPAPAD.Common.Instrumentation;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private static async Task Main(string[] args)
		{
			SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
			new SilentRegistrationStartup().Intialize();
			try
			{
				Program._logger = LoggingManager.CreateLogger<Program>(null);
				SilentRegistrationCommands silentRegistrationCommands = new SilentRegistrationCommands(args.ToList<string>());
				if (silentRegistrationCommands.OperationToExecute == SilentRegistrationOperationType.Help)
				{
					SilentRegistrationCommands.DisplayUsage(false);
				}
				else
				{
					Program.InitCorrelationInformations(silentRegistrationCommands);
					Program.ExitIfNotFirstInstance();
					if (silentRegistrationCommands.OperationToExecute == SilentRegistrationOperationType.GetRegistrationState)
					{
						await Program.PerformRegistrationStateOperationAsync();
					}
					else
					{
						Program.SetupAuthenticationService(silentRegistrationCommands);
						await Program.PerformRegistrationWorkAsync(silentRegistrationCommands);
					}
				}
			}
			catch (Exception ex)
			{
				Program.LogAndDisplayError(ex);
				Environment.Exit(-1);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002094 File Offset: 0x00000294
		private static void InitCorrelationInformations(SilentRegistrationCommands silentRegistrationCommands)
		{
			string text;
			RequestCorrelationContext.Current.CorrelationId = (silentRegistrationCommands.TryGetCorrelationId(out text) ? text : Guid.NewGuid().ToString());
			string text2;
			RequestCorrelationContext.Current.ClientSessionId = (silentRegistrationCommands.TryGetClientSessionId(out text2) ? text2 : Guid.NewGuid().ToString());
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020F8 File Offset: 0x000002F8
		private static void ExitIfNotFirstInstance()
		{
			if (!ServiceLocator.Current.GetInstance<ISingleApplicationInstanceService>().IsFirstInstance())
			{
				Program._logger.Warning("ExitIfNotFirstInstance", LogEvent.MachineRegistration.Init, "Silent registration app exited early because another instance is already running.", null, -1L, Array.Empty<LogData>());
				Console.Error.WriteLine(Resources.RegApp_OtherInstanceRunning_Message);
				Environment.Exit(0);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002154 File Offset: 0x00000354
		private static void LogAndDisplayError(Exception exception)
		{
			if (exception == null)
			{
				return;
			}
			string text = SilentRegistrationErrorCodes.Unknown.ToString();
			string text2 = "Unhandled exception of type " + exception.GetType().FullName + ". Please see the logs for more details.";
			SilentRegistrationException ex = exception as SilentRegistrationException;
			if (ex != null)
			{
				text = ex.ErrorCode;
				text2 = string.Join(Environment.NewLine, new string[] { ex.ErrorCode, ex.Message });
			}
			Program._logger.Error("Main", LogEvent.MachineRegistration, "Silent registration failed with error code: " + text + ".", exception, -1L, Array.Empty<LogData>());
			Console.Error.WriteLine(string.Join(Environment.NewLine, new string[]
			{
				text2,
				"Correlation id: " + RequestCorrelationContext.Current.CorrelationId
			}));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002228 File Offset: 0x00000428
		private static async Task PerformRegistrationStateOperationAsync()
		{
			await ServiceLocator.Current.GetInstance<ISilentRegistrationOperations>().GetMachineRegistrationStateAsync();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002264 File Offset: 0x00000464
		private static async Task PerformRegistrationWorkAsync(SilentRegistrationCommands silentRegistrationCommands)
		{
			ISilentRegistrationOperations silentRegistrationOperations = ServiceLocator.Current.GetInstance<ISilentRegistrationOperations>();
			if (silentRegistrationCommands.OperationToExecute != SilentRegistrationOperationType.RegisterHosted && silentRegistrationCommands.OperationToExecute != SilentRegistrationOperationType.AADJoin)
			{
				Program.SetupAuthenticationService(silentRegistrationCommands);
				await silentRegistrationOperations.TryLogonAsync(silentRegistrationCommands.GetAuthenticationFallback()).ConfigureAwait(false);
				await silentRegistrationOperations.SetServicePlanDetailsAsync().ConfigureAwait(false);
				await silentRegistrationOperations.SetCurrentEnvironmentAsync(silentRegistrationCommands.OperationToExecute, silentRegistrationCommands.GetEnvironmentIdentifier()).ConfigureAwait(false);
			}
			switch (silentRegistrationCommands.OperationToExecute)
			{
			case SilentRegistrationOperationType.Register:
				await silentRegistrationOperations.RegisterMachineAsync(silentRegistrationCommands.GetRegistrationMachineNameParameter(), silentRegistrationCommands.GetRegistrationMachineDescriptionParameter(), silentRegistrationCommands.NeedForceRegistration()).ConfigureAwait(false);
				return;
			case SilentRegistrationOperationType.JoinGroup:
			{
				Guid guid;
				SecureString secureString;
				if (!silentRegistrationCommands.TryGetJoinGroupParameters(out guid, out secureString))
				{
					SilentRegistrationCommands.DisplayUsage(true);
					throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingJoinGroupParameters.ToString(), "Missing join machine group parameters. See -help above.", null);
				}
				await silentRegistrationOperations.JoinMachineGroupAsync(guid, secureString).ConfigureAwait(false);
				return;
			}
			case SilentRegistrationOperationType.GetStatus:
				await silentRegistrationOperations.GetStatusAsync(silentRegistrationCommands.GetFormatType()).ConfigureAwait(false);
				return;
			case SilentRegistrationOperationType.RegisterHosted:
			{
				Uri uri;
				string text;
				SecureString secureString2;
				SecureString secureString3;
				if (!silentRegistrationCommands.TryGetRegisterHostedParameters(out uri, out text, out secureString2, out secureString3))
				{
					throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingRegisterHostedParameters.ToString(), "Missing register hosted parameters.", null);
				}
				await silentRegistrationOperations.RegisterHostedMachineAsync(uri, text, secureString2, secureString3, silentRegistrationCommands.GetRegistrationMachineNameParameter(), silentRegistrationCommands.GetRegistrationMachineDescriptionParameter());
				return;
			}
			case SilentRegistrationOperationType.AADJoin:
			{
				Uri uri2;
				string text2;
				SecureString secureString4;
				string text3;
				if (!silentRegistrationCommands.TryGetAADJoinParameters(out uri2, out text2, out secureString4, out text3))
				{
					throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingAADJoinParameters.ToString(), "Missing AAD join parameters.", null);
				}
				await silentRegistrationOperations.AADJoinDeviceAsync(uri2, secureString4, text2, text3);
				return;
			}
			case SilentRegistrationOperationType.Recover:
				await silentRegistrationOperations.RecoverMachineAsync().ConfigureAwait(false);
				return;
			}
			SilentRegistrationCommands.DisplayUsage(true);
			throw new SilentRegistrationException(SilentRegistrationErrorCodes.OperationNotSupported.ToString(), string.Format("Invalid operation '{0}' to execute. See -help above.", silentRegistrationCommands.OperationToExecute), null);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000022A8 File Offset: 0x000004A8
		private static void SetupAuthenticationService(SilentRegistrationCommands silentRegistrationCommands)
		{
			CloudType cloudType;
			if (!silentRegistrationCommands.TryGetCloudType(out cloudType))
			{
				cloudType = CloudType.Public;
			}
			CloudInfo cloudInfo = new CloudInfo(cloudType, null);
			string tenantIdentifier = silentRegistrationCommands.GetTenantIdentifier();
			IMicrosoftAuthenticationServiceManager instance = ServiceLocator.Current.GetInstance<IMicrosoftAuthenticationServiceManager>();
			IRegistryValuesService instance2 = ServiceLocator.Current.GetInstance<IRegistryValuesService>();
			bool flag = false;
			string text;
			if (!silentRegistrationCommands.TryGetApplicationIdentifier(out text))
			{
				instance.Initialize(cloudInfo.InstanceEndpoint, cloudInfo.ApplicationId, cloudInfo.RedirectEndpoint, tenantIdentifier ?? "organizations", instance2.PadSettings.UseMsalDesktopFeatures_IsEnabled(), instance2.PadSettings.UseMsalWindowsBroker_IsEnabled());
			}
			else
			{
				SecureString secureString = null;
				string text2 = null;
				flag = true;
				if (string.IsNullOrEmpty(tenantIdentifier) || (!silentRegistrationCommands.TryGetApplicationSecret(out secureString) && !silentRegistrationCommands.TryGetApplicationCertificateThumbprint(out text2)))
				{
					SilentRegistrationCommands.DisplayUsage(true);
					throw new SilentRegistrationException(SilentRegistrationErrorCodes.MissingServicePrincipalParameters.ToString(), "Missing parameters to authenticate a service principal account. See -help above.", null);
				}
				IMicrosoftAuthenticationServiceManager microsoftAuthenticationServiceManager = instance;
				string instanceEndpoint = cloudInfo.InstanceEndpoint;
				string text3 = text;
				string redirectEndpoint = cloudInfo.RedirectEndpoint;
				string text4 = tenantIdentifier;
				SecureString secureString2 = secureString;
				microsoftAuthenticationServiceManager.InitializeConfidential(instanceEndpoint, text3, redirectEndpoint, text4, text2, secureString2);
			}
			string text5;
			silentRegistrationCommands.TryGetUsernameParameter(out text5);
			text5 = text5 ?? string.Empty;
			IUserAccountInfo userAccountInfo = ServiceLocator.Current.GetInstance<IConsoleRoot>().UserAccountInfo;
			string text6 = text5;
			string text7 = text5;
			string empty = string.Empty;
			CloudInfo cloudInfo2 = cloudInfo;
			bool flag2 = flag;
			userAccountInfo.SetAccountInfo(text6, text7, empty, cloudInfo2, tenantIdentifier, AccountType.Undefined, flag2);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000023DC File Offset: 0x000005DC
		private static void <Main>(string[] args)
		{
			Program.Main(args).GetAwaiter().GetResult();
		}

		// Token: 0x04000001 RID: 1
		private static ILogger<Program> _logger;

		// Token: 0x04000002 RID: 2
		private const int EXIT_CODE_ERROR = -1;
	}
}
