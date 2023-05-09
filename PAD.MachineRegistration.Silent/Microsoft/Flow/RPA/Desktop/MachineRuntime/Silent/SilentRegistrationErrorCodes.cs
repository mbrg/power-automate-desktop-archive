using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{

	public enum SilentRegistrationErrorCodes
	{

		Unknown,

		EnvironmentNotFound,

		GroupTypeNotSupported,

		OperationNotSupported,

		MissingParameters,

		InvalidArguments,

		InvalidCloudType,

		InvalidGuid,

		CanNotExecuteMultipleOperations,

		MissingServicePrincipalParameters,

		MissingJoinGroupParameters,

		PasswordStrengthIsTooWeak,

		SecureInputAsParameterIsForbidden,

		FailJoinGroup,

		InvalidAuthenticationFallbackType,

		AuthenticationFailed,

		InternalError,

		CdsError,

		MachineAlreadyRegistered,

		SilentOperationAlreadyInProgress,

		InvalidFormatType,

		EnvironmentDenied,

		MissingRegisterHostedParameters,

		MissingAADJoinParameters,

		InvalidTargetEndpoint,

		RelayConnectionStateNotConnected
	}
}
