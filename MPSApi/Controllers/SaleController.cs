using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MPSDataManager.library.Models;
using MPSDataMananger.library.DataAccess;
using MPSDataMananger.library.Models;
using System.Security.Claims;

namespace MPSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _config;
        public SaleController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize(Roles = "Cashier")]
        [HttpPost]
        public void Post(SaleModel sale)
        {

            SaleData data = new SaleData(_config);
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier); // old way RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userID);

        }
        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
        [HttpGet]
        public List<SaleReportModel> GetSalesReport()
        {
            //if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    //do admin stuff
            //}
            //else if (RequestContext.Principal.IsInRole("Admin"))
            //{
            //    //do manager stuff
            //}
            SaleData data = new SaleData(_config);
            return data.GetSaleReposts();
        }


    }
}

