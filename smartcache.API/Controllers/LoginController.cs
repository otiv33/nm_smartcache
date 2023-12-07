using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using smartcache.API.Exceptions;
using smartcache.API.Auth;
using smartcache.API.Models;


namespace smartcache.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Index([FromBody] UnathorizedUser user)
        {
            if (user.Name == "User" && user.Password == "123")
            {
                var service = new JwtService(_configuration);
                string token = service.GenerateToken(user.Name, "User");
                return Ok(new { token = token });
            }
            else
            {
                throw new InvalidCredentialsException("User credentials are false");
            }
        }
    }
}
