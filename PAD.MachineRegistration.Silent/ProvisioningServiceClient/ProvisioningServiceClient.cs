using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Flow.RPA.Desktop.Common.Services.HttpClients.Factory;
using Microsoft.Flow.RPA.Desktop.Shared.Clients.Common;
using Microsoft.Flow.RPA.Desktop.Shared.Logging;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Data;
using Microsoft.Flow.RPA.Desktop.Shared.Logging.Common.Interfaces;
using Newtonsoft.Json;

namespace PAD.MachineRegistration.Silent.ProvisioningServiceClient
{
	// Token: 0x0200001C RID: 28
	public class ProvisioningServiceClient : IProvisioningServiceClient, IDisposable
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00004B9E File Offset: 0x00002D9E
		public ProvisioningServiceClient(IHttpClientFactory httpClientFactory, IHttpRequestMessageFactory httpRequestMessageFactory, ILogger<ProvisioningServiceClient> logger)
		{
			this._httpClient = httpClientFactory.CreateHttpClientInstance();
			this._httpRequestMessageFactory = httpRequestMessageFactory;
			this._logger = logger;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004BC0 File Offset: 0x00002DC0
		private string GetAadJoinHostedMachineUrl(Uri serviceUri)
		{
			return string.Format("{0}{1}", serviceUri, "hostedMachines/aadjoin");
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004BD2 File Offset: 0x00002DD2
		private static HttpContent GetJsonHttpContent(AADJoinHostedMachineRequest bodyObject)
		{
			return new StringContent(JsonConvert.SerializeObject(bodyObject), Encoding.UTF8, "application/json");
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004BEC File Offset: 0x00002DEC
		public async Task<AADJoinHostedMachineResponse> AADJoinHostedMachineRequestAsync(Uri serviceUri, string authToken, AADJoinHostedMachineRequest aadJoinRequest, CancellationToken cancellationToken = default(CancellationToken))
		{
			ProvisioningServiceClient.<>c__DisplayClass12_0 CS$<>8__locals1 = new ProvisioningServiceClient.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.serviceUri = serviceUri;
			CS$<>8__locals1.authToken = authToken;
			CS$<>8__locals1.aadJoinRequest = aadJoinRequest;
			CS$<>8__locals1.cancellationToken = cancellationToken;
			CS$<>8__locals1.correlationIds = null;
			CS$<>8__locals1.requestIds = null;
			CS$<>8__locals1.statusCode = (HttpStatusCode)0;
			return await this._logger.TraceOperationAsync("AADJoinHostedMachineRequestAsync", new Func<Task<AADJoinHostedMachineResponse>>(CS$<>8__locals1.<AADJoinHostedMachineRequestAsync>g__AADJoinHostedMachineRequestInternalAsync|0), delegate(AADJoinHostedMachineResponse result)
			{
				string text = "Call finished with status code {0}, requestId {1}, correlationId {2}, deviceId {3}";
				object[] array = new object[4];
				array[0] = CS$<>8__locals1.statusCode;
				int num = 1;
				IEnumerable<string> requestIds = CS$<>8__locals1.requestIds;
				array[num] = ((requestIds != null) ? requestIds.FirstOrDefault<string>() : null);
				int num2 = 2;
				IEnumerable<string> correlationIds = CS$<>8__locals1.correlationIds;
				array[num2] = ((correlationIds != null) ? correlationIds.FirstOrDefault<string>() : null);
				array[3] = ((result != null) ? result.DeviceId : null);
				return string.Format(text, array);
			}, delegate(Exception exception)
			{
				string text2 = "Call finished on error with status code {0}, requestId {1}, correlationId {2}, exception {3}";
				object[] array2 = new object[4];
				array2[0] = CS$<>8__locals1.statusCode;
				int num3 = 1;
				IEnumerable<string> requestIds2 = CS$<>8__locals1.requestIds;
				array2[num3] = ((requestIds2 != null) ? requestIds2.FirstOrDefault<string>() : null);
				int num4 = 2;
				IEnumerable<string> correlationIds2 = CS$<>8__locals1.correlationIds;
				array2[num4] = ((correlationIds2 != null) ? correlationIds2.FirstOrDefault<string>() : null);
				array2[3] = exception;
				return string.Format(text2, array2);
			}, Array.Empty<LogData>());
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004C50 File Offset: 0x00002E50
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposedValue)
			{
				if (disposing)
				{
					HttpClient httpClient = this._httpClient;
					if (httpClient != null)
					{
						httpClient.Dispose();
					}
				}
				this._disposedValue = true;
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004C75 File Offset: 0x00002E75
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000098 RID: 152
		private const string AadJoinMachineRoute = "hostedMachines/aadjoin";

		// Token: 0x04000099 RID: 153
		private const string ServiceRequestIdHeader = "x-ms-service-request-id";

		// Token: 0x0400009A RID: 154
		private const string ServiceCorrelationIdHeader = "x-ms-correlation-id";

		// Token: 0x0400009B RID: 155
		private const string JsonMediaType = "application/json";

		// Token: 0x0400009C RID: 156
		private static readonly List<MediaTypeWithQualityHeaderValue> AcceptJsonHeader = new List<MediaTypeWithQualityHeaderValue>
		{
			new MediaTypeWithQualityHeaderValue("application/json")
		};

		// Token: 0x0400009D RID: 157
		private readonly HttpClient _httpClient;

		// Token: 0x0400009E RID: 158
		private readonly IHttpRequestMessageFactory _httpRequestMessageFactory;

		// Token: 0x0400009F RID: 159
		private readonly ILogger<ProvisioningServiceClient> _logger;

		// Token: 0x040000A0 RID: 160
		private bool _disposedValue;
	}
}
