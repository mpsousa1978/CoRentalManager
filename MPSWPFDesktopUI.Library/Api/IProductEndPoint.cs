using MPSWPFDesktopUI.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MPSWPFDesktopUI.Library.Api
{
    public interface IProductEndPoint
    {
        Task<List<ProductModel>> GetAll();
    }
}