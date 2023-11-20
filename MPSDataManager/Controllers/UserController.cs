using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MPSDataMananger.library.Models;
using MPSDataMananger.Library.DataAccess;

namespace MPSDataManager.Controllers
{
    [Authorize]
    
    public class UserController : ApiController
    {

        public string Get(int id)
        {
            return "value";
        }

        // GET: User/Details/5
        public List<UserModel> GetById()
        {

            string userID = RequestContext.Principal.Identity.GetUserId();

            UserData data = new UserData();

            return data.GetUserById(userID);
        }

        
    }
}
