using MPSWPFDesktopUI.Models;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Helper
{
    public interface IApiHelper
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
    }
}