using MPSWPFDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Helper
{
    public class ApiHelper : IApiHelper
    {


        private HttpClient apiClient;

        public ApiHelper()
        {
            InitializeApiClente();
        }
        private void InitializeApiClente()
        {
            string api = ConfigurationManager.AppSettings["Api"]; //App.config

            apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(api);
            apiClient.DefaultRequestHeaders.Accept.Clear();
            apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task<AuthenticatedUserModel> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("username",username),
                new KeyValuePair<string,string>("password",password)
            });

            using (HttpResponseMessage response = await apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<AuthenticatedUserModel>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }

            }
        }
    }
}
