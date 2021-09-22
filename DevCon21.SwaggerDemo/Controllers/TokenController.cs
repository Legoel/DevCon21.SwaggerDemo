using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevCon21.SwaggerDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DevCon21.SwaggerDemo.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly TodoListContext _context;
        private readonly IConfiguration _configuration;

        public TokenController(TodoListContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<UserToken> GetToken(string login, string password)
        {
            if (!ValidateUser(login, password))
                return Unauthorized();

            var expiresOn = DateTime.UtcNow.AddMinutes(_configuration.GetValue("Authentication:JwtToken:Expiration", 60));

            var userToken = new UserToken
            {
                Login = login,
                ExpiresOn = expiresOn,
                Roles = GetRoles(login)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var secret = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Authentication:JwtToken:Secret"));
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetClaims(login)),
                Expires = expiresOn,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(descriptor);
            userToken.Token = tokenHandler.WriteToken(token);
            return Ok(userToken);
        }

        private bool ValidateUser(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
                return false;

            if (string.IsNullOrEmpty(password))
                return false;

            var user = _context.People.SingleOrDefault(p => p.FullName == login);

            if (user is null)
                return false;

            return password.Equals("Password");
        }

        private static IEnumerable<string> GetRoles(string login)
        {
            yield return login.Equals("Thomas", StringComparison.OrdinalIgnoreCase) ? "Administrator" : "Contributor";
        }

        private static IEnumerable<Claim> GetClaims(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, login),
                new Claim(ClaimTypes.Name, login)
            };

            claims.AddRange(GetRoles(login).Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }
    }
}
