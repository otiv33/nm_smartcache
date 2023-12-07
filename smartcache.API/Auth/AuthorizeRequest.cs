using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IdentityModel.Tokens.Jwt;
using smartcache.API.Exceptions;
using smartcache.API.Models;

namespace smartcache.API.Auth
{


    public class AuthorizeRequest : ActionFilterAttribute
    {
        private string[] roles;

        public AuthorizeRequest(params string[] roles)
        {
            this.roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string auth = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(auth))
            {
                throw new InvalidCredentialsException("Credentials are empty");
            }

            User? user = GetUser(auth);
            if (!IsAuthorized(user) || user == null)
            {
                throw new UnathorizedException("User does not have the proper permissions");
            }
        }


        private bool IsAuthorized(User? user)
        {
            return roles.FirstOrDefault(r => r == user?.Role, null) != null;
        }

        private User GetUser(string jwtToken)
        {
            jwtToken = jwtToken.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
                JwtPayload payload = jwtSecurityToken.Payload;
                User user = new User()
                {
                    Name = (string)payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                    Role = (string)payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
                };
                return user;
            }
            catch
            {
                throw new Exception("Decoding the token failed.");
            }
        }

    }

}
