
using MPSWPFDesktopUI.Library.Models;
using MPSWPFDesktopUI.Models;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Library.Api
{
    public interface IApiHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
        Task GetLoggedUserInfo(string token);
    }
}