using Newtonsoft.Json;
using Printlaser.Infra.Crosscutting.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Printlaser.Infra.Crosscutting.Rest
{
    public class RestHelper
    {
        #region GetAsync
        public (bool success, string message, TResult result) GetAsync<TResult>(RestApi restApi, string relativeUrl, bool bearerAuth = false)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                SetBearerToken(restApi, bearerAuth, httpClient);

                var result = httpClient.GetAsync($"{GetBaseUrl(restApi)}{relativeUrl}").Result;

                if (!result.IsSuccessStatusCode)
                    return (false, "Falha na conexão com a API.", default(TResult));

                return (true, string.Empty, JsonConvert.DeserializeObject<TResult>(result.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return (false, ex.Message, default(TResult));
            }
        }
        #endregion

        #region PostAsync
        public (bool success, string message) PostAsync<TModel>(RestApi restApi, string relativeUrl, TModel model, bool bearerAuth = false)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                SetBearerToken(restApi, bearerAuth, httpClient);
                HttpResponseMessage result = PostAsync(restApi, relativeUrl, model, httpClient);

                if (!result.IsSuccessStatusCode)
                    return (false, "Falha na conexão com a API.");

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool success, string message, TResult result) PostAsync<TModel, TResult>(RestApi restApi, string relativeUrl, TModel model, bool bearerAuth = false)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                SetBearerToken(restApi, bearerAuth, httpClient);
                HttpResponseMessage result = PostAsync(restApi, relativeUrl, model, httpClient);

                if (!result.IsSuccessStatusCode)
                    return (false, "Falha na conexão com a API.", default(TResult));

                return (true, string.Empty, JsonConvert.DeserializeObject<TResult>(result.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return (false, ex.Message, default(TResult));
            }
        }
        #endregion

        #region PutAsync
        public (bool success, string message) PutAsync<TModel>(RestApi restApi, string relativeUrl, TModel model, bool bearerAuth = false)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                SetBearerToken(restApi, bearerAuth, httpClient);
                HttpResponseMessage result = PutAsync(restApi, relativeUrl, model, httpClient);

                if (!result.IsSuccessStatusCode)
                    return (false, "Falha na conexão com a API.");

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool success, string message, TResult result) PutAsync<TModel, TResult>(RestApi restApi, string relativeUrl, TModel model, bool bearerAuth = false)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                SetBearerToken(restApi, bearerAuth, httpClient);
                HttpResponseMessage result = PutAsync(restApi, relativeUrl, model, httpClient);

                if (!result.IsSuccessStatusCode)
                    return (false, "Falha na conexão com a API.", default(TResult));

                return (true, string.Empty, JsonConvert.DeserializeObject<TResult>(result.Content.ReadAsStringAsync().Result));
            }
            catch (Exception ex)
            {
                return (false, ex.Message, default(TResult));
            }
        }
        #endregion

        #region Private Methods
        private HttpResponseMessage PostAsync<TModel>(RestApi restApi, string relativeUrl, TModel model, HttpClient httpClient)
        {
            return httpClient.PostAsync($"{GetBaseUrl(restApi)}{relativeUrl}", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")).Result;
        }

        private HttpResponseMessage PutAsync<TModel>(RestApi restApi, string relativeUrl, TModel model, HttpClient httpClient)
        {
            return httpClient.PutAsync($"{GetBaseUrl(restApi)}{relativeUrl}", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json")).Result;
        }

        private void SetBearerToken(RestApi restApi, bool bearerAuth, HttpClient httpClient)
        {
            if (bearerAuth)
            {
                var authToken = new AuthenticationHelper().ObterAuthToken(GetClientId(restApi), GetClientSecret(restApi));
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", string.Format("Bearer {0}", authToken.Access_token));
            }
        }


        private string GetBaseUrl(RestApi restApi)
        {
            switch (restApi)
            {
                case RestApi.SSO:
#if DEBUG
                    return "http://localhost:59159/api/";
#else
                    return "https://login.printlaser.com/api/";
#endif
                case RestApi.CMP:
                    return "http://api.printlaser.com/cmp/api/";

                case RestApi.ProcessamentoHistorico:
                    return "https://5xciujskpg.execute-api.sa-east-1.amazonaws.com/Prod/";

                case RestApi.Remessa:
                    return "https://api-cob.pld.sh/cobranca/";

                case RestApi.PortalDigital:
                    return "http://dados.din.printlaser.com/";

                case RestApi.Email:
                    return "http://email.din.printlaser.com/";

                default:
                    return string.Empty;
            }
        }

        private string GetClientId(RestApi restApi)
        {
            switch (restApi)
            {
                case RestApi.SSO:
                    return "62e69329a75c47788c258165c4c3e3c7";
                case RestApi.CMP:
                    return "13d99450-738b-438d-863c-85c4f2c29369";
                case RestApi.ProcessamentoHistorico:
                    return string.Empty;
                case RestApi.Remessa:
                    return string.Empty;
                case RestApi.PortalDigital:
                    return string.Empty;
                case RestApi.Email:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }

        private string GetClientSecret(RestApi restApi)
        {
            switch (restApi)
            {
                case RestApi.SSO:
                    return "vSptiezCTSeVAiZnTwj4jlRcBJvKmyWFOjv38AMT9Dw";
                case RestApi.CMP:
                    return "zwVr8bKt39KsA_ZcBA51HbWg_5dkFFODUAtThRHe-6s";
                case RestApi.ProcessamentoHistorico:
                    return string.Empty;
                case RestApi.Remessa:
                    return string.Empty;
                case RestApi.PortalDigital:
                    return string.Empty;
                case RestApi.Email:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
