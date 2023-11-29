using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MPSApi.Data;
using MPSApi.Models;
using MPSDataMananger.library.Models;
using MPSDataMananger.Library.DataAccess;
using System.Security.Claims;

namespace MPSApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public UserController(ApplicationDbContext context,UserManager<IdentityUser> userManager, 
            IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config= config;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier); //RequestContext.Principal.Identity.GetUserId();
            UserData data = new UserData(_config);

            return data.GetUserById(userID).First();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();
            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };
                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);
                output.Add(u);

        }
            return output;
        }

        [Authorize(Roles = "Admin")]
        [Route("Admin/GetAllRoles")]
        [HttpGet]
        public Dictionary<string, string> GetAllRoles()
        {
                var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);
                return roles;
        }

        [Authorize(Roles = "Admin")]
        [Route("Admin/AddRole")]
        [HttpPost]
        public async Task AddRole(UserRolePairModel paring)
        {
            var user = await _userManager.FindByIdAsync(paring.UserId);

            await _userManager.AddToRoleAsync(user, paring.RoleName);
        }


        [Authorize(Roles = "Admin")]
        [Route("Admin/RemoveRole")]
        [HttpPost]
        public async Task RemoveRole(UserRolePairModel paring)
        {
            var user = await _userManager.FindByIdAsync(paring.UserId);

            await _userManager.RemoveFromRoleAsync(user, paring.RoleName);
        }
    }
}
