using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MPSDataMananger.Library.DataAccess;
using MPSDataMananger.library.Models;
using MPSDataManager.Models;
using System.Runtime.Remoting.Contexts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MPSDataManager.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        [HttpGet]
        public UserModel GetById()
        {
            string userID = RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData();

            return data.GetUserById(userID).First();
        }

        [Authorize (Roles ="Admin")]
        [HttpGet]
        [Route("Api/User/Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {

            List<ApplicationUserModel> output = new List<ApplicationUserModel>();
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var users = userManager.Users.ToList();
                var roles = context.Roles.ToList();

                foreach(var user in users)
                {
                    ApplicationUserModel u = new ApplicationUserModel();
                    u.Id    = user.Id;
                    u.Email = user.Email;

                    foreach (var r in user.Roles)
                    {
                        u.Roles.Add(r.RoleId,roles.Where(x => x.Id == r.RoleId).FirstOrDefault().Name);
                    }
                    output.Add(u);
                }

            }
            return output;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Api/User/Admin/GetAllRoles")]
        public Dictionary<string,string> GetAllRoles()
        {
            using (var context = new ApplicationDbContext())
            {
                var roles = context.Roles.ToDictionary(x => x.Id, x=>x.Name);
                return roles;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Api/User/Admin/AddRole")]
        public void AddRole(UserRolePairModel paring)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);

                userManager.AddToRole(paring.UserId, paring.RoleName);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Api/User/Admin/RemoveRole")]
        public void RemoveRole(UserRolePairModel paring)
        {
            using (var context = new ApplicationDbContext())
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                userManager.RemoveFromRole(paring.UserId, paring.RoleName);

            }
        }

    }
}
