using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MPSApi.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MPSApi.Controllers
{
    public class TokenController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TokenController(ApplicationDbContext context,UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("/Token")]
        [HttpPost]
        public async Task<IActionResult> Create(string username, string password,string grant_type)
        {
            if (await IsValidusernameAndPassword(username, password))
            {
                return new ObjectResult(await  GenerateToken(username));
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<bool> IsValidusernameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);

        }
        //JWT
        private async Task<dynamic> GenerateToken(string username)
        {
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds().ToString())
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretSODoNotTellMySecretKeyIsSecretSODoNotTell"));

            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(key, SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));


            //var token = new JwtSecurityToken(
            //    new JwtHeader(
            //        new SigningCredentials(
            //            new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretSODoNotTell")),
            //        SecurityAlgorithms.HmacSha256)),
            //    new JwtPayload(claims));


            try
            {
                var output = new
                {
                    Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserName = username
                };

                return output;
            }
            catch (Exception)
            {

                throw ;
            }


            return null ;

        }
    }
}
