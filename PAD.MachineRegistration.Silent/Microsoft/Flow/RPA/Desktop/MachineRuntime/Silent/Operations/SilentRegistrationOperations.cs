using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Flow.RPA.Desktop.Common.Core.Domain.Machine.Enums;
using Microsoft.Flow.RPA.Desktop.Common.Services.Registry;
using Microsoft.Flow.RPA.Desktop.Console.Application.Persistence.Repositories.Environments;
using Microsoft.Flow.RPA.Desktop.Console.Application.ServicePlan;
using Microsoft.Flow.RPA.Desktop.Console.Application.ServicePlan.Commands;
using Microsoft.Flow.RPA.Desktop.Console.Core.Environment;
using Microsoft.Flow.RPA.Desktop.Console.Core.Services;
using Microsoft.Flow.RPA.Desktop.Console.Core.UserAccount;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Application.Extensions;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Application.Machine;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Application.Machine.Commands;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Application.Persistence;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Core.ApplicationRoot;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Core.Machine;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Authentication;
using Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.SilentRegistrationCommandParameters;
using Microsoft.Flow.RPA.Desktop.Shared.Clients.Common.Repos.Cds;
using Microsoft.Flow.RPA.Desktop.Shared.Clients.DataClients.Cds;
using Microsoft.Flow.RPA.Desktop.Shared.Clients.DataClients.Dto;
using Microsoft.Flow.RPA.Desktop.Shared.Common.CloudInfoEntities;
using Microsoft.Flow.RPA.Desktop.Shared.Common.Extensions;
using Microsoft.Flow.RPA.Desktop.Shared.Logging;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Data;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Microsoft.Flow.RPA.Desktop.Shared.MsAccountAuthenticator;
using Microsoft.Flow.RPA.Desktop.Shared.Services;
using Microsoft.Flow.RPA.RegistrationContract;
using Microsoft.Flow.RPAPAD.Common.Json;
using Microsoft.Flow.RPAPAD.Common.RpcOverNamedPipes.SharedUtility;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Operations
{

	public class SilentRegistrationOperations : ISilentRegistrationOperations
	{

		public SilentRegistrationOperations(IMachineRuntimeUnitOfWork unitOfWork, IMediator mediator, IStorageTypeService storageTypeService, IMachineRegistrationManager machineRegistrationManager, ILoggerContext loggerContext, ICdsClient cdsClient, ITokenProvider tokenProvider, IMicrosoftAuthenticationServiceManager microsoftAuthenticationServiceManager, IMachineRuntimeRoot machineRuntimeRoot, IRegistryValuesService registryValuesService, ILogger<SilentRegistrationOperations> logger, IRegisterHostedMachineOperation registerHostedMachineOperation, IAADJoinDeviceOperation aadJoinDeviceOperation)
		{
			this._environmentsRepositoryProxy = unitOfWork.Environments;
			this._mediator = mediator;
			this._machineRegistrationManager = machineRegistrationManager;
			this._loggerContext = loggerContext;
			this._cdsClient = cdsClient;
			this._tokenProvider = tokenProvider;
			this._microsoftAuthenticationServiceManager = microsoftAuthenticationServiceManager;
			this._userAccountInfo = machineRuntimeRoot.UserAccountInfo;
			this._registryValuesService = registryValuesService;
			this._logger = logger;
			this._registerHostedMachineOperation = registerHostedMachineOperation;
			this._aadJoinDeviceOperation = aadJoinDeviceOperation;
			storageTypeService.Set(StorageType.Cds);
		}


		public async Task TryLogonAsync(SilentRegistrationAuthenticationFallbackType silentRegistrationAuthenticationFallbackType)
		{
			SilentRegistrationOperations.<>c__DisplayClass21_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass21_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.silentRegistrationAuthenticationFallbackType = silentRegistrationAuthenticationFallbackType;
			await this._logger.TraceOperationAsync("TryLogonAsync", new Func<Task>(CS$<>8__locals1.<TryLogonAsync>g__TryLogonInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task JoinMachineGroupAsync(Guid groupId, SecureString password)
		{
			SilentRegistrationOperations.<>c__DisplayClass22_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass22_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.groupId = groupId;
			CS$<>8__locals1.password = password;
			CS$<>8__locals1.groupId.ThrowIfEmpty("groupId", null);
			CS$<>8__locals1.password.ThrowIfNull("password", null);
			if (!this._machineState.IsRegisteredState())
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.FailJoinGroup.ToString(), "You cannot join a group if the machine is not registered in an environment.", null);
			}
			if (!this._currentEnvironment.ApiUrl.Equals(this._currentRegisteredEnvironment.ApiUrl))
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.FailJoinGroup.ToString(), "You cannot join the group because this machine is currently registered in another environment: " + this._currentRegisteredEnvironment.Name, null);
			}
			await this._logger.TraceOperationAsync("JoinMachineGroupAsync", new Func<Task>(CS$<>8__locals1.<JoinMachineGroupAsync>g__JoinMachineGroupInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task RegisterMachineAsync(string machineName, string machineDescription, bool overrideExistingRegistration)
		{
			SilentRegistrationOperations.<>c__DisplayClass23_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass23_0();
			CS$<>8__locals1.overrideExistingRegistration = overrideExistingRegistration;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.machineName = machineName;
			CS$<>8__locals1.machineDescription = machineDescription;
			await this._logger.TraceOperationAsync("RegisterMachineAsync", new Func<Task>(CS$<>8__locals1.<RegisterMachineAsync>g__RegisterMachineInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task RegisterHostedMachineAsync(Uri serviceUri, string vmResourceId, SecureString miAuthToken, SecureString groupPassword, string machineName, string machineDescription)
		{
			SilentRegistrationOperations.<>c__DisplayClass24_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass24_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.serviceUri = serviceUri;
			CS$<>8__locals1.vmResourceId = vmResourceId;
			CS$<>8__locals1.miAuthToken = miAuthToken;
			CS$<>8__locals1.groupPassword = groupPassword;
			CS$<>8__locals1.machineName = machineName;
			CS$<>8__locals1.machineDescription = machineDescription;
			await this._logger.TraceOperationAsync("RegisterHostedMachineAsync", new Func<Task>(CS$<>8__locals1.<RegisterHostedMachineAsync>g__RegisterHostedMachineInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task RegisterRpaBoxAsync(string b64SerializedRegisterRpaBoxRequest)
		{
			SilentRegistrationOperations.<>c__DisplayClass25_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass25_0();
			CS$<>8__locals1.b64SerializedRegisterRpaBoxRequest = b64SerializedRegisterRpaBoxRequest;
			CS$<>8__locals1.<>4__this = this;
			await this._logger.TraceOperationAsync("RegisterRpaBoxAsync", new Func<Task>(CS$<>8__locals1.<RegisterRpaBoxAsync>g__RegisterRpaBoxInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task AADJoinDeviceAsync(Uri serviceUri, SecureString miAuthToken, string vmResourceId, string tenantId)
		{
			SilentRegistrationOperations.<>c__DisplayClass26_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.serviceUri = serviceUri;
			CS$<>8__locals1.miAuthToken = miAuthToken;
			CS$<>8__locals1.vmResourceId = vmResourceId;
			CS$<>8__locals1.tenantId = tenantId;
			await this._logger.TraceOperationAsync("AADJoinDeviceAsync", new Func<Task>(CS$<>8__locals1.<AADJoinDeviceAsync>g__AADJoinDeviceInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task SetServicePlanDetailsAsync()
		{
			await this._logger.TraceOperationAsync("SetServicePlanDetailsAsync", new Func<Task>(this.<SetServicePlanDetailsAsync>g__SetServicePlanDetailsInternalAsync|27_0), null, null, Array.Empty<LogData>());
		}


		public async Task SetCurrentEnvironmentAsync(SilentRegistrationOperationType operationType, string environmentId = null)
		{
			SilentRegistrationOperations.<>c__DisplayClass28_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass28_0();
			CS$<>8__locals1.environmentId = environmentId;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.operationType = operationType;
			await this._logger.TraceOperationAsync("SetCurrentEnvironmentAsync", new Func<Task>(CS$<>8__locals1.<SetCurrentEnvironmentAsync>g__SetCurrentEnvironmentInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task GetStatusAsync(SilentRegistrationFormatType formatType)
		{
			SilentRegistrationOperations.<>c__DisplayClass29_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass29_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.formatType = formatType;
			await this._logger.TraceOperationAsync("GetStatusAsync", new Func<Task>(CS$<>8__locals1.<GetStatusAsync>g__GetStatusInternalAsync|0), null, null, Array.Empty<LogData>());
		}


		public async Task GetMachineRegistrationStateAsync()
		{
			await this._logger.TraceOperationAsync("GetMachineRegistrationStateAsync", new Func<Task>(this.<GetMachineRegistrationStateAsync>g__GetMachineRegistrationStateInternalAsync|30_0), null, null, Array.Empty<LogData>());
		}


		public async Task RecoverMachineAsync()
		{
			await this._logger.TraceOperationAsync("RecoverMachineAsync", new Func<Task>(this.<RecoverMachineAsync>g__RecoverMachineInternalAsync|31_0), null, null, Array.Empty<LogData>());
		}


		private async Task<Guid> GetUserIdAsync()
		{
			Guid guid;
			try
			{
				guid = await this._logger.TraceOperationAsync("GetUserIdAsync", new Func<Task<Guid>>(this.<GetUserIdAsync>g__GetUserIdInternalAsync|32_0), null, null, Array.Empty<LogData>());
			}
			catch (CdsClientException ex)
			{
				throw SilentRegistrationOperations.ToSilentRegistrationException(ex);
			}
			return guid;
		}


		private async Task<CdsMachineGroupDto> GetMachineGroupAsync(Guid groupId)
		{
			SilentRegistrationOperations.<>c__DisplayClass33_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.groupId = groupId;
			CdsMachineGroupDto cdsMachineGroupDto;
			try
			{
				cdsMachineGroupDto = await this._logger.TraceOperationAsync("GetMachineGroupAsync", new Func<Task<CdsMachineGroupDto>>(CS$<>8__locals1.<GetMachineGroupAsync>g__GetMachineGroupInternalAsync|0), null, null, Array.Empty<LogData>());
			}
			catch (CdsClientException ex)
			{
				throw SilentRegistrationOperations.ToSilentRegistrationException(ex);
			}
			return cdsMachineGroupDto;
		}


		private async Task JoinMachineGroupWithRetryAsync(Guid groupId, SecureString password, int attempt)
		{
			SilentRegistrationOperations.<>c__DisplayClass34_0 CS$<>8__locals1 = new SilentRegistrationOperations.<>c__DisplayClass34_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.password = password;
			CS$<>8__locals1.attempt = attempt;
			CS$<>8__locals1.groupId = groupId;
			if (CS$<>8__locals1.attempt > 3)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.FailJoinGroup.ToString(), string.Format("Failed to join machine group '{0}'. Please retry or contact support.", CS$<>8__locals1.groupId), null);
			}
			CdsMachineGroupDto cdsMachineGroupDto = await this.GetMachineGroupAsync(CS$<>8__locals1.groupId).ConfigureAwait(false);
			CS$<>8__locals1.targetGroup = cdsMachineGroupDto;
			if (CS$<>8__locals1.targetGroup == null)
			{
				throw new SilentRegistrationException(RegistrationErrorCode.GroupNotFound.ToString(), string.Format("Cannot find machine group '{0}' in environment '{1}'.", CS$<>8__locals1.groupId, this._currentEnvironment.Id), null);
			}
			if (!CS$<>8__locals1.targetGroup.CanJoin)
			{
				throw new SilentRegistrationException(RegistrationErrorCode.CannotJoinGroup.ToString(), string.Format("Missing permissions: Cannot join machine group '{0}' in environment '{1}'.", CS$<>8__locals1.groupId, this._currentEnvironment.Id), null);
			}
			await CS$<>8__locals1.password.DecryptSecureStringAndUseResultAsStringAsync(new Func<string, Task>(CS$<>8__locals1.<JoinMachineGroupWithRetryAsync>g__UseUnsecureString|0));
		}


		private async Task TryFallbackLogonAsync(SilentRegistrationAuthenticationFallbackType silentRegistrationAuthenticationFallbackType)
		{
			CloudInfo cloudInfo = this._userAccountInfo.CloudInfo;
			AudiencePermissions[] array = new AudiencePermissions[]
			{
				new AudiencePermissions(cloudInfo.Flow.Audience, new string[] { cloudInfo.Flow.FlowReadAllPermission })
			};
			try
			{
				if (silentRegistrationAuthenticationFallbackType != SilentRegistrationAuthenticationFallbackType.Interactive)
				{
					if (silentRegistrationAuthenticationFallbackType == SilentRegistrationAuthenticationFallbackType.DeviceCode)
					{
						await this._microsoftAuthenticationServiceManager.Service.GetAccessTokenWithDeviceCodeAsync(array, this._userAccountInfo.Email, new Action<DeviceCodeResult>(SilentRegistrationOperations.<TryFallbackLogonAsync>g__DeviceCodeCallback|35_0), default(CancellationToken)).ConfigureAwait(false);
					}
				}
				else
				{
					await this._microsoftAuthenticationServiceManager.Service.GetAccessTokenByEmailWithUiPromptAsync(array, this._userAccountInfo.Email, default(CancellationToken)).ConfigureAwait(false);
				}
			}
			catch (MsalException ex)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.AuthenticationFailed.ToString(), string.Join(System.Environment.NewLine, new string[]
				{
					string.Format("Failed to acquire token using '{0}' fallback.", silentRegistrationAuthenticationFallbackType),
					"Error code: " + ex.ErrorCode + ". Error message: " + ex.Message
				}), ex);
			}
		}


		private void CheckRelayConnectionStateAsync(SilentRegistrationOperationType operationType)
		{
			if (!this._machineState.IsRegisteredState())
			{
				return;
			}
			RelayConnectionState relayConnectionState;
			if (Enum.TryParse<RelayConnectionState>(this._currentRegistrationStatus.RelayConnectionState, out relayConnectionState) && relayConnectionState != RelayConnectionState.Connected && operationType != SilentRegistrationOperationType.Recover)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.RelayConnectionStateNotConnected.ToString(), "Your machine is not connected. Verify your network connection or recover your machine.\nUse -recover command to do so.", null);
			}
		}


		private static SilentRegistrationException ToSilentRegistrationException(CdsClientException cdsException)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Format("Error code: {0}. Http status code: {1}.", cdsException.ErrorCode, cdsException.HttpStatusCode));
			if (cdsException.RequestIds != null)
			{
				IEnumerable<string> enumerable = from pair in cdsException.RequestIds
					where pair.Value != null
					select pair.Key + ":" + string.Join(";", pair.Value);
				stringBuilder.Append(" Request ids: " + string.Join(",", enumerable) + ".");
			}
			return new SilentRegistrationException(SilentRegistrationErrorCodes.CdsError.ToString(), stringBuilder.ToString(), cdsException);
		}


		private static void HandleBaseMachineCommandsResult(BaseMachineCommandsResult result, string defaultMessage)
		{
			if (result.IsSuccess)
			{
				return;
			}
			string text = SilentRegistrationErrorCodes.InternalError.ToString();
			string text2 = defaultMessage;
			if (result.Error != null)
			{
				text = result.Error.Code.ToString();
				RegistrationErrorCode code = result.Error.Code;
				if (code != RegistrationErrorCode.MachineAlreadyRegistered)
				{
					switch (code)
					{
					case RegistrationErrorCode.NoPayGoNoLicenseError:
						text2 = "Cannot register your machine in this environment. You must have a valid per user plan with attended RPA or your environment must be pay-as-you-go.";
						goto IL_BD;
					case RegistrationErrorCode.UnauthorizedTenantSwitching:
						text2 = "Registering in this environment would change your machine's tenant which is disallowed by default. Check https://go.microsoft.com/fwlink/?linkid=2204956 to learn why and how to override the default behavior.";
						goto IL_BD;
					case RegistrationErrorCode.UnauthorizedRegistrationToUnjoinedTenant:
						text2 = "You are trying to register to a tenant that doesn't match your machine's AAD tenant, which is disallowed by default. Check https://go.microsoft.com/fwlink/?linkid=2204956 to learn why and how to override the default behavior.";
						goto IL_BD;
					case RegistrationErrorCode.UnauthorizedRegistrationToNonAllowListedTenant:
						text2 = "You are trying to register to an environment that isn't in the registration tenant allow-list of this machine. Check https://go.microsoft.com/fwlink/?linkid=2204956 to learn more about the registration tenant allow-list.";
						goto IL_BD;
					}
					text2 = (string.IsNullOrWhiteSpace(result.Error.Message) ? text2 : result.Error.Message);
				}
				else
				{
					text2 = "The machine is already registered. Please specify '-force' parameter to override existing registration.";
				}
			}
			IL_BD:
			throw new SilentRegistrationException(text, text2, result.UnexpectedException);
		}


		private void DisplayOnConsoleDefault(GetStatusResponse response, bool groupRetrieved)
		{
			string text = "\tError retrieving information" + System.Environment.NewLine;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(System.Environment.NewLine + "RegistrationDetails:" + System.Environment.NewLine);
			if (response.RegistrationDetails == null)
			{
				stringBuilder.Append(text);
			}
			else
			{
				stringBuilder.Append(this.GetDisplayableStringFromDictionnary(new Dictionary<string, string>
				{
					{
						"EnvironmentId",
						response.RegistrationDetails.EnvironmentId
					},
					{
						"MachineId",
						response.RegistrationDetails.MachineId
					},
					{
						"RegistrationState",
						response.RegistrationDetails.RegistrationState
					},
					{
						"RelayConnectionState",
						response.RegistrationDetails.RelayConnectionState
					}
				}));
			}
			if (this._machineState.IsRegisteredState())
			{
				stringBuilder.Append(System.Environment.NewLine + "MachineDetails:" + System.Environment.NewLine);
				if (response.MachineDetails == null)
				{
					stringBuilder.Append(text);
				}
				else
				{
					stringBuilder.Append(this.GetDisplayableStringFromDictionnary(new Dictionary<string, string>
					{
						{
							"Name",
							response.MachineDetails.Name
						},
						{
							"Description",
							response.MachineDetails.Description
						},
						{
							"OwnerFullname",
							response.MachineDetails.OwnerFullname
						},
						{
							"CanView",
							response.MachineDetails.CanView.ToString()
						},
						{
							"CanEdit",
							response.MachineDetails.CanEdit.ToString()
						}
					}));
				}
				if (response.GroupDetails != null || !groupRetrieved)
				{
					stringBuilder.Append(System.Environment.NewLine + "GroupDetails:" + System.Environment.NewLine);
					if (response.GroupDetails == null)
					{
						stringBuilder.Append(text);
					}
					else
					{
						stringBuilder.Append(this.GetDisplayableStringFromDictionnary(new Dictionary<string, string>
						{
							{
								"Name",
								response.GroupDetails.Name
							},
							{
								"MachineCount",
								response.GroupDetails.MachineCount.ToString()
							},
							{
								"OwnerFullname",
								response.GroupDetails.OwnerFullname
							},
							{
								"CanJoin",
								response.GroupDetails.CanJoin.ToString()
							}
						}));
					}
				}
			}
			Console.WriteLine(stringBuilder.ToString());
		}


		private string GetDisplayableStringFromDictionnary(Dictionary<string, string> dict)
		{
			ValueTuple<int, int> maxLengths = this.GetMaxLengths(dict);
			int item = maxLengths.Item1;
			int num = maxLengths.Item2;
			int num2 = Math.Min(80, item + num + 7);
			num = num2 - item - 7;
			StringBuilder stringBuilder = new StringBuilder();
			string text = string.Concat(new string[]
			{
				"| {0, -",
				item.ToString(),
				"} | {1, -",
				num.ToString(),
				"} |",
				System.Environment.NewLine
			});
			stringBuilder.Append('-', num2);
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append(string.Format(text, "Name", "Value"));
			stringBuilder.Append('-', num2);
			stringBuilder.Append(System.Environment.NewLine);
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				string text2 = keyValuePair.Value ?? "null";
				string text3 = string.Format(text, keyValuePair.Key, text2.Substring(0, Math.Min(text2.Length, num)));
				while (text2.Length > num)
				{
					stringBuilder.Append(text3);
					text2 = text2.Substring(num);
					text3 = string.Format(text, string.Empty, text2.Substring(0, Math.Min(text2.Length, num)));
				}
				stringBuilder.Append(text3);
			}
			stringBuilder.Append('-', num2);
			stringBuilder.Append(System.Environment.NewLine);
			return stringBuilder.ToString();
		}


		[return: TupleElementNames(new string[] { "maxKeyLength", "maxValueLength" })]
		private ValueTuple<int, int> GetMaxLengths(Dictionary<string, string> dict)
		{
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<string, string> keyValuePair in dict)
			{
				string key = keyValuePair.Key;
				string value = keyValuePair.Value;
				num = Math.Max(num, keyValuePair.Key.Length);
				int num3 = num2;
				string value2 = keyValuePair.Value;
				num2 = Math.Max(num3, (value2 != null) ? value2.Length : "null".Length);
			}
			return new ValueTuple<int, int>(num, num2);
		}


		[CompilerGenerated]
		private async Task <SetServicePlanDetailsAsync>g__SetServicePlanDetailsInternalAsync|27_0()
		{
			if (!this._userAccountInfo.IsServicePrincipal)
			{
				await this._mediator.Send<ServicePlansDetailsDto>(new RetrieveServicePlanDetailsCommand(), default(CancellationToken));
			}
		}


		[CompilerGenerated]
		private async Task <GetMachineRegistrationStateAsync>g__GetMachineRegistrationStateInternalAsync|30_0()
		{
			try
			{
				Console.WriteLine(JsonConvert.SerializeObject(await this._machineRegistrationManager.GetMachineStatusAsync(default(CancellationToken)).ConfigureAwait(false), JsonSettings.ObjectSerializationSettings));
			}
			catch (Exception ex)
			{
				throw new SilentRegistrationException(SilentRegistrationErrorCodes.InternalError.ToString(), "Cannot retrieve MachineRegistrationStatus", ex);
			}
		}


		[CompilerGenerated]
		private async Task <RecoverMachineAsync>g__RecoverMachineInternalAsync|31_0()
		{
			SilentRegistrationOperations.HandleBaseMachineCommandsResult(await this._mediator.Send<RecoverMachineCommandResult>(new RecoverMachineCommand(this._currentOrgUri, this._currentEnvironment.Id), default(CancellationToken)), "Failed to recover machine in environment '" + this._currentEnvironment.Id + "'. Please retry or contact support.");
		}


		[CompilerGenerated]
		private async Task<Guid> <GetUserIdAsync>g__GetUserIdInternalAsync|32_0()
		{
			TokenProviderResultDto tokenProviderResultDto = await this._tokenProvider.GetAccessTokenAsync(this._currentOrgUri.AbsoluteUri, "user_impersonation", default(CancellationToken)).ConfigureAwait(false);
			return (await this._cdsClient.GetWhoAmIAsync(this._currentOrgUri, tokenProviderResultDto.Token, null, default(CancellationToken)).ConfigureAwait(false)).UserId;
		}


		[CompilerGenerated]
		internal static void <TryFallbackLogonAsync>g__DeviceCodeCallback|35_0(DeviceCodeResult deviceCodeResult)
		{
			Console.WriteLine(deviceCodeResult.Message);
		}


		private const int MaxJoinMachineGroupAttempt = 3;


		private const string NullReadableString = "null";


		private Uri _currentOrgUri;


		private Microsoft.Flow.RPA.Desktop.Console.Core.Environment.Environment _currentEnvironment;


		private Microsoft.Flow.RPA.Desktop.Console.Core.Environment.Environment _currentRegisteredEnvironment;


		private RegistrationStatusResponse _currentRegistrationStatus;


		private RegistrationState _machineState;


		private Guid _currentMachineGroupId;


		private readonly IEnvironmentsRepositoryProxy _environmentsRepositoryProxy;


		private readonly IMediator _mediator;


		private readonly IMachineRegistrationManager _machineRegistrationManager;


		private readonly ILoggerContext _loggerContext;


		private readonly ICdsClient _cdsClient;


		private readonly ITokenProvider _tokenProvider;


		private readonly IMicrosoftAuthenticationServiceManager _microsoftAuthenticationServiceManager;


		private readonly IUserAccountInfo _userAccountInfo;


		private readonly IRegistryValuesService _registryValuesService;


		private readonly ILogger<SilentRegistrationOperations> _logger;


		private readonly IRegisterHostedMachineOperation _registerHostedMachineOperation;


		private readonly IAADJoinDeviceOperation _aadJoinDeviceOperation;
	}
}
