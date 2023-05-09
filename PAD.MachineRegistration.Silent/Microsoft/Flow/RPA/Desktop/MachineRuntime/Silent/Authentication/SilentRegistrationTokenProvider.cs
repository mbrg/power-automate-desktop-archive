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

	public class SilentRegistrationTokenProvider : ITokenProvider
	{

		public SilentRegistrationTokenProvider(IMicrosoftAuthenticationServiceManager microsoftAuthenticationServiceManager, IConsoleRoot consoleRoot)
		{
			this._microsoftAuthenticationServiceManager = microsoftAuthenticationServiceManager;
			this._userAccountInfo = consoleRoot.UserAccountInfo;
		}


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


		public Task<TokenProviderResultDto> GetAccessTokenForTenantAsync(string audienceUrl, string tenantId, string permission = "user_impersonation", CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}


		public Task<string> GetFlowsAccessTokenAsync(string permission = "user_impersonation", CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}


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


		public Task<string> GetOneDriveAppTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}


		public Task<string> GetTenantListTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotImplementedException();
		}


		private readonly IMicrosoftAuthenticationServiceManager _microsoftAuthenticationServiceManager;


		private readonly IUserAccountInfo _userAccountInfo;
	}
}
