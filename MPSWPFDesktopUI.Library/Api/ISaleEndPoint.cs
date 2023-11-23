using MPSWPFDesktopUI.Library.Models;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Library.Api
{
    public interface ISaleEndPoint
    {
        Task PostSale(SaleModel sale);
    }
}