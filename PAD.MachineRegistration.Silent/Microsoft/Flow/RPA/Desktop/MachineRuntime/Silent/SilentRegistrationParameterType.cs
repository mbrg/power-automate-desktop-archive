using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public enum SilentRegistrationParameterType
	{

		Undefined,

		Username,

		ForceRegistration,

		RegistrationMachineName,

		RegistrationMachineDescription,

		RegistrationRegion,

		JoinGroup,

		LeaveGroup,

		SetRunEnvironment,

		UpdateGroup,

		UpdateGroupName,

		UpdateGroupDescription,

		EditPassword,

		Unregister,

		CloudType,

		ApplicationIdentifier,

		ApplicationSecret,

		ApplicationCertificateThumbprint,

		EnvironmentIdentifier,

		Register,

		GroupId,

		GroupPassword,

		TenantIdentifier,

		AuthenticationFallback,

		Usage,

		GetStatus,

		FormatType,

		CorrelationId,

		ClientSessionId,

		GetRegistrationState,

		RegisterHosted,

		ManagedIdentityAuthToken,

		VmResourceId,

		ServiceUri,

		AADJoin,

		Recover
	}
}
