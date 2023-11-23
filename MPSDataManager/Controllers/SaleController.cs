
using Microsoft.AspNet.Identity;
using MPSDataManager.library.Models;
using MPSDataManager.Models;
using MPSDataMananger.library.DataAccess;
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
        public void Post(SaleModel sale)
        {

            SaleData data = new SaleData();
            string userID = RequestContext.Principal.Identity.GetUserId();
            data.SaveSale(sale, userID);

        }
    }
}
