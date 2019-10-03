using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Printlaser.Infra.Crosscutting
{
    public class URLWork
    {
        public static string ConverterUrl(string txt)
        {
            Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);

            foreach (Match match in mactches)
            {
                string tURL = GerarUrlCurta(match.Value);
                txt = txt.Replace(match.Value, tURL);
            }

            return txt;
        }

        public static string GerarUrlCurta(string Url)
        {
            try
            {

                if (Url.Length <= 12)
                {
                    return Url;
                }
                if (!Url.ToLower().StartsWith("http") && !Url.ToLower().StartsWith("ftp"))
                {
                    Url = "http://" + Url;
                }

#if DEBUG
                WebProxy proxy = new WebProxy("server02:8080", false)
                {
                    UseDefaultCredentials = false,
                    Credentials = CredentialCache.DefaultNetworkCredentials,
                };

                HttpClientHandler httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                    PreAuthenticate = true,
                    UseDefaultCredentials = false,
                };

                httpClientHandler.Credentials = CredentialCache.DefaultNetworkCredentials;

                using (var client = new System.Net.WebClient())
                //using (var client = new HttpClient(httpClientHandler))
                {
                    WebRequest.DefaultWebProxy = proxy;
                    client.Proxy = proxy;
#else
                using (var client = new System.Net.WebClient())
                //using (var client = new HttpClient())
                {
#endif

                    //string strUrlShortner = "";
                    //client.BaseAddress = new Uri("https://www.googleapis.com/urlshortener/v1/");
                    //client.DefaultRequestHeaders.Add("cache-control", "no-cache");

                    //var content = new StringContent("{\"longUrl\": \""+ Url + "\"}", Encoding.UTF8, "application/json");

                    //var gapi = client.PostAsync("url?key=AIzaSyC6swOfFT3nGCPJxmYCRAAHXzr33Ri_vKo", content);
                    //gapi.Wait();

                    //if (gapi.Result.StatusCode == HttpStatusCode.OK)
                    //{
                    //    strUrlShortner = gapi.Result.Content.ReadAsStringAsync().Result;
                    //}

                    //return strUrlShortner;

                    var request = WebRequest.Create("http://tinyurl.com/api-create.php?url=" + Url);

                    string text = null;
                    bool status = false;

                    while (!status)
                    {
                        var res = request.GetResponse();

                        using (var reader = new StreamReader(res.GetResponseStream()))
                        {
                            text = reader.ReadToEnd();
                        }

                        if (!text.StartsWith("http://tinyurl.com/"))
                            throw new Exception("Erro ao gerar URL.");
                        else
                            status = true;
                    }
                    return text;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }


    }

    public class GoogleShortner
    {
        public string kind { get; set; }
        public string id { get; set; }
        public string longUrl { get; set; }
    }
}
