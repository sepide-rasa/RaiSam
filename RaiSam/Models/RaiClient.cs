using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using RaiSam.Models;

namespace RaiSam.Models
{
    internal class TokenRequest
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
    internal class RaiToken
    {
        public int expires_in { get; set; }
        public string access_token { get; set; }
    }
    public interface IRaiClient
    {
        Task<ReturnMessage<companyOut>> InquiryByNationalCode(User user, Input_Company model, CancellationToken cancellationToken);
        Task<ReturnMessage<OutputDto_SabtAhvalSitad>> RegistryOfficeInquiry(User user, InputDto_SabtAhval model, CancellationToken cancellationToken);
        Task SetAccessToken(User user, CancellationToken cancellationToken);
    }
    public class RaiClient : IRaiClient
    {
        private static Lazy<ApiClient> restClient;
        private DateTime tokenExpireDateTime = DateTime.Now;

        public RaiClient(string URL)
        {
            restClient = new Lazy<ApiClient>(() => new ApiClient(new Uri(URL)), LazyThreadSafetyMode.ExecutionAndPublication);
        }
        public async Task SetAccessToken(User user, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(restClient.Value.ACCESS_TOKEN)
                && tokenExpireDateTime > DateTime.Now)
            {
                await Task.CompletedTask.ConfigureAwait(false);
            }
            else
            {
                var model = new TokenRequest
                {
                    grant_type = "password",
                    username = user.UserName,
                    password = user.Password/*,
                    refresh_token = String.Empty,
                    scope = String.Empty,
                    client_id = String.Empty,
                    client_secret = String.Empty,*/
                };

                var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
                  "api/v1/Users/TokenBody"));
                var accessToken = restClient.Value.PostAsync<RaiToken, TokenRequest>(requestUrl, model, cancellationToken);
                accessToken.Wait();
                tokenExpireDateTime = DateTime.Now.AddSeconds(accessToken.Result.expires_in);
                restClient.Value.ACCESS_TOKEN = accessToken.Result.access_token;
            }
        }
        public async Task<ReturnMessage<companyOut>> InquiryByNationalCode(User user, Input_Company model, CancellationToken cancellationToken)
        {
            await SetAccessToken(user, cancellationToken).ConfigureAwait(false);
            var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "/api/v1/LegalPerson/InquiryByNationalCode"));

            var dto = restClient.Value.PostAsync2<companyOut, Input_Company>(requestUrl, model, cancellationToken);
            dto.Wait();
            return dto.Result;
        }
        public async Task<ReturnMessage<companyOut>> InquiryByName(User user, Input_Company model, CancellationToken cancellationToken)
        {
            await SetAccessToken(user, cancellationToken).ConfigureAwait(false);
            var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "/api/v1/LegalPerson/InquiryByName"));

            var dto = restClient.Value.PostAsync2<companyOut, Input_Company>(requestUrl, model, cancellationToken);
            dto.Wait();
            return dto.Result;
        }
        public async Task<ReturnMessage<OutputDto_SabtAhvalSitad>> RegistryOfficeInquiry(User user, InputDto_SabtAhval model, CancellationToken cancellationToken)
        {
            await SetAccessToken(user, cancellationToken).ConfigureAwait(false);
            var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "/api/v1/Sitaad/RegistryOfficeInquiry"));

            var dto = restClient.Value.PostAsync2<OutputDto_SabtAhvalSitad, InputDto_SabtAhval>(requestUrl, model, cancellationToken);
            dto.Wait();
            return dto.Result;
        }
        public async Task<ReturnMessage<OutputDto_Address>> GetAddressByPostcode(User user, InputDto_Address model, CancellationToken cancellationToken)
        {
            await SetAccessToken(user, cancellationToken).ConfigureAwait(false);
            var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "/api/v1/Sitaad/GetAddressByPostcode"));

            var dto = restClient.Value.PostAsync2<OutputDto_Address, InputDto_Address>(requestUrl, model, cancellationToken);
            dto.Wait();
            return dto.Result;
        }
        public async Task<ReturnMessage<Output_Mosaferi>> GetMosaferi(User user, Input_Mosaferi model, CancellationToken cancellationToken)
        {
            await SetAccessToken(user, cancellationToken).ConfigureAwait(false);
            var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "/api/v1/Made12/Mosaferi"));

            var dto = restClient.Value.PostAsync2<Output_Mosaferi, Input_Mosaferi>(requestUrl, model, cancellationToken);
            dto.Wait();
            return dto.Result;
        }
        public async Task<ReturnMessage<MoveTranDetails>> GetMoveTranDetails(User user, Input_Sejam model, CancellationToken cancellationToken)
        {
            await SetAccessToken(user, cancellationToken).ConfigureAwait(false);
            var requestUrl = restClient.Value.CreateRequestUri(string.Format(System.Globalization.CultureInfo.InvariantCulture,
            "/api/v1/PCS/GetMoveTranDetails"));

            var dto = restClient.Value.PostAsync2<MoveTranDetails, Input_Sejam>(requestUrl, model, cancellationToken);
            dto.Wait();
            return dto.Result;
        }
    }
}