using Microsoft.AspNet.Identity;
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
    public class ProductController : ApiController
    {
        [HttpGet]
        public List<ProductModel> Get()
        {
            
            ProductData product = new ProductData();
            return product.GetProducts();
        }
    }
}
