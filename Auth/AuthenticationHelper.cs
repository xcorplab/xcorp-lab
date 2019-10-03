using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Printlaser.Infra.Crosscutting.Auth
{
    public class AuthenticationHelper
    {
        public RetornoToken ObterAuthToken(string clientId, string clientSecret)
        {
            try
            {
                HttpClient httpClient = new HttpClient();

                //var byteArray = Encoding.ASCII.GetBytes("13d99450-738b-438d-863c-85c4f2c29369:zwVr8bKt39KsA_ZcBA51HbWg_5dkFFODUAtThRHe-6s");
                var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpResponseMessage response = new HttpResponseMessage();
                for (int i = 0; i < 5; i++)
                {
                    var content = new StringContent("Content-Type", Encoding.UTF8, "application/json");
                    response = httpClient.PostAsync("http://login.printlaser.com/oauth/token", content).Result;
                    if (response.IsSuccessStatusCode)
                        break;
                }
                return JsonConvert.DeserializeObject<RetornoToken>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
