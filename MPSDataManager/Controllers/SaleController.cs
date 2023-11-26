
using Microsoft.AspNet.Identity;
using MPSDataManager.library.Models;
using MPSDataManager.Models;
using MPSDataMananger.library.DataAccess;
using MPSDataMananger.library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;



namespace MPSDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier")]
        public void Post(SaleModel sale)
        {

            SaleData data = new SaleData();
            string userID = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userID);

        }
        [Authorize(Roles = "Admin,Manager")]
        [Route("GetSalesReport")]
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
            SaleData data = new SaleData();
            return data.GetSaleReposts();
        }


    }
}
