using System;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent
{
	// Token: 0x02000006 RID: 6
	public enum SilentRegistrationErrorCodes
	{
		// Token: 0x04000038 RID: 56
		Unknown,
		// Token: 0x04000039 RID: 57
		EnvironmentNotFound,
		// Token: 0x0400003A RID: 58
		GroupTypeNotSupported,
		// Token: 0x0400003B RID: 59
		OperationNotSupported,
		// Token: 0x0400003C RID: 60
		MissingParameters,
		// Token: 0x0400003D RID: 61
		InvalidArguments,
		// Token: 0x0400003E RID: 62
		InvalidCloudType,
		// Token: 0x0400003F RID: 63
		InvalidGuid,
		// Token: 0x04000040 RID: 64
		CanNotExecuteMultipleOperations,
		// Token: 0x04000041 RID: 65
		MissingServicePrincipalParameters,
		// Token: 0x04000042 RID: 66
		MissingJoinGroupParameters,
		// Token: 0x04000043 RID: 67
		PasswordStrengthIsTooWeak,
		// Token: 0x04000044 RID: 68
		SecureInputAsParameterIsForbidden,
		// Token: 0x04000045 RID: 69
		FailJoinGroup,
		// Token: 0x04000046 RID: 70
		InvalidAuthenticationFallbackType,
		// Token: 0x04000047 RID: 71
		AuthenticationFailed,
		// Token: 0x04000048 RID: 72
		InternalError,
		// Token: 0x04000049 RID: 73
		CdsError,
		// Token: 0x0400004A RID: 74
		MachineAlreadyRegistered,
		// Token: 0x0400004B RID: 75
		SilentOperationAlreadyInProgress,
		// Token: 0x0400004C RID: 76
		InvalidFormatType,
		// Token: 0x0400004D RID: 77
		EnvironmentDenied,
		// Token: 0x0400004E RID: 78
		MissingRegisterHostedParameters,
		// Token: 0x0400004F RID: 79
		MissingAADJoinParameters,
		// Token: 0x04000050 RID: 80
		InvalidTargetEndpoint,
		// Token: 0x04000051 RID: 81
		RelayConnectionStateNotConnected
	}
}
