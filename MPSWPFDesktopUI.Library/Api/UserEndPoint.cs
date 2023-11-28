using MPSWPFDesktopUI.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Library.Api
{
    public class UserEndPoint : IUserEndPoint
    {
        private readonly IApiHelper _apiHelper;
        public UserEndPoint(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public async Task<List<UserModel>> GetAll()
        {

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/user/admin/GetAllUsers"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<UserModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Dictionary<string,string>> GetAllRoles()
        {

            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/user/admin/GetAllRoles"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Dictionary<string,string>>();
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task AddUserToRole(string userId,string roleName)
        {
            var data = new { userId, roleName };

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/user/admin/AddRole",data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task RemoveFromRole(string userId, string roleName)
        {
            var data = new { userId, roleName };

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/user/admin/RemoveRole", data))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }


    }
}
