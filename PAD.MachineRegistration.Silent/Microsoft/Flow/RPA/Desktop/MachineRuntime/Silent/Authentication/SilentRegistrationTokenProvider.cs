using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.Console.Core.ApplicationRoot;
using Microsoft.Flow.RPA.Desktop.Console.Core.Services;
using Microsoft.Flow.RPA.Desktop.Console.Core.UserAccount;
using Microsoft.Flow.RPA.Desktop.Shared.Clients.TenantDiscovery.Entities;
using Microsoft.Flow.RPA.Desktop.Shared.MsAccountAuthenticator;
using Microsoft.Flow.RPA.Desktop.Shared.MsAccountAuthenticator.Entities;

namespace Microsoft.Flow.RPA.Desktop.MachineRuntime.Silent.Authentication
{
	// Token: 0x02000018 RID: 24
	public class SilentRegistrationTokenProvider : ITokenProvider
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00004A58 File Offset: 0x00002C58
		public SilentRegistrationTokenProvider(IMicrosoftAuthenticationServiceManager microsoftAuthenticationServiceManager, IConsoleRoot consoleRoot)
		{
			this._microsoftAuthenticationServiceManager = microsoftAuthenticationServiceManager;
			this._userAccountInfo = consoleRoot.UserAccountInfo;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004A74 File Offset: 0x00002C74
		public async Task<TokenProviderResultDto> GetAccessTokenAsync(string audienceUrl, string permission = "user_impersonation", CancellationToken cancellationToken = default(CancellationToken))
		{
			TokenProviderResultDto tokenProviderResultDto;
			if (this._userAccountInfo.IsServicePrincipal)
			{
				AudiencePermissions[] array = new AudiencePermissions[]
				{
					new AudiencePermissions(audienceUrl, new string[] { ".default" })
				};
				MicrosoftAuthenticationResult microsoftAuthenticationResult = await this._microsoftAuthenticationServiceManager.Service.GetAccessTokenForClient(array, cancellationToken).ConfigureAwait(false);
				tokenProviderResultDto = new TokenProviderResultDto(microsoftAuthenticationResult.AccessToken, microsoftAuthenticationResult.TenantId);
			}
			else
			{
				IEnumerable<AudiencePermissions> enumerable = new AudiencePermissions[]
				{
					new AudiencePermissions(audienceUrl, new string[] { permission })
				};
				MicrosoftAuthenticationResult microsoftAuthenticationResult2 = await this._microsoftAuthenticationServiceManager.Service.GetAccessTokenByIntegratedWindowsAuthAsync(enumerable, this._userAccountInfo.Email, cancellationToken).ConfigureAwait(false);
				tokenProviderResultDto = new TokenProviderResultDto(microsoftAuthenticationResult2.AccessToken, microsoftAuthenticationResult2.TenantId);
			}
			return tokenProviderResultDto;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00004ACF File Offset: 0x00002CCF
		public Task<TokenProviderResultDto> GetAccessTokenForTenantAsync(string audienceUrl, string tenantId, string permission = "user_impersonation", CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004AD6 File Offset: 0x00002CD6
		public Task<string> GetFlowsAccessTokenAsync(string permission = "user_impersonation", CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00004AE0 File Offset: 0x00002CE0
		public async Task<string> GetGlobalDiscoTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			CloudInfo cloudInfo = this._userAccountInfo.CloudInfo;
			string text;
			if (this._userAccountInfo.IsServicePrincipal)
			{
				AudiencePermissions[] array = new AudiencePermissions[]
				{
					new AudiencePermissions(cloudInfo.Flow.Audience, new string[] { ".default" })
				};
				text = (await this._microsoftAuthenticationServiceManager.Service.GetAccessTokenForClient(array, cancellationToken).ConfigureAwait(false)).AccessToken;
			}
			else
			{
				IEnumerable<AudiencePermissions> enumerable = new AudiencePermissions[]
				{
					new AudiencePermissions(cloudInfo.Flow.Audience, new string[] { cloudInfo.Flow.FlowReadAllPermission })
				};
				text = (await this._microsoftAuthenticationServiceManager.Service.GetAccessTokenByIntegratedWindowsAuthAsync(enumerable, this._userAccountInfo.Email, cancellationToken).ConfigureAwait(false)).AccessToken;
			}
			return text;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00004B2B File Offset: 0x00002D2B
		public Task<string> GetOneDriveAppTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004B32 File Offset: 0x00002D32
		public Task<string> GetTenantListTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}

		// Token: 0x04000091 RID: 145
		private readonly IMicrosoftAuthenticationServiceManager _microsoftAuthenticationServiceManager;

		// Token: 0x04000092 RID: 146
		private readonly IUserAccountInfo _userAccountInfo;
	}
}
