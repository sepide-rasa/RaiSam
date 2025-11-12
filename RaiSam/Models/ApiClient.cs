using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace RaiSam.Models
{
    [DataContract]
    public class ReturnMessage<T>
    {
        [DataMember(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "StatusCode")]
        public ApiResultStatusCode StatusCode { get; set; }

        [DataMember(Name = "Message")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [DataMember(Name = "Data")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [DataMember(Name = "BinaryDataPdf")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BinaryDataPdf { get; set; }



        [DataMember(Name = "StudentUniInfo")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T StudentUniInfo { get; set; }

        [DataMember(Name = "StudentMsrtInfo")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T StudentMsrtInfo { get; set; }

        [DataMember(Name = "StudentInfo")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T StudentInfo { get; set; }
    }
    public class ReturnMessage2<T>
    {
        [DataMember(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "StatusCode")]
        public ApiResultStatusCode StatusCode { get; set; }

        [DataMember(Name = "Message")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [DataMember(Name = "Data")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<T> Data { get; set; }

    }
    public enum ApiResultStatusCode
    {
        OK = 1,
        NotOK = 2
    }
    public interface IApiClient
    {
        Task<ReturnMessage<T1>> PostAsync2<T1, T2>(Uri requestUrl, T2 content, CancellationToken cancellationToken);
        Task<T1> PostAsync<T1, T2>(Uri requestUrl, T2 content, CancellationToken cancellationToken);
    }
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }
        public string ACCESS_TOKEN = String.Empty;

        public ApiClient(Uri baseEndpoint)
        {
            if (baseEndpoint == null)
            {
                throw new ArgumentNullException("baseEndpoint");
            }
            BaseEndpoint = baseEndpoint;
            _httpClient = new HttpClient();
        }
        public async Task<T1> PostAsync<T1, T2>(Uri requestUrl, T2 content, CancellationToken cancellationToken)
        {
            addHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content), cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ACCESS_TOKEN = String.Empty;
            }
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T1>(data);
        }
        public async Task<ReturnMessage<T1>> PostAsync2<T1, T2>(Uri requestUrl, T2 content, CancellationToken cancellationToken)
        {
            addHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content), cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ACCESS_TOKEN = String.Empty;
            }
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ReturnMessage<T1>>(data);
        }
        public async Task<ReturnMessage2<T1>> PostAsync3<T1, T2>(Uri requestUrl, T2 content, CancellationToken cancellationToken)
        {
            addHeaders();
            var response = await _httpClient.PostAsync(requestUrl.ToString(), CreateHttpContent<T2>(content), cancellationToken).ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                ACCESS_TOKEN = String.Empty;
            }
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<ReturnMessage2<T1>>(data);
        }
        public Uri CreateRequestUri(string relativePath, string queryString = "")
        {
            var endpoint = new Uri(BaseEndpoint, relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }
        private HttpContent CreateHttpContent<T>(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        private static JsonSerializerSettings MicrosoftDateFormatSettings
        {
            get
            {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }
        private void addHeaders()
        {
            if (ACCESS_TOKEN != String.Empty)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);
            }

        }
    }
}