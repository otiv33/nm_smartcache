using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using smartcache.API.Models;

namespace smartcache.API.Controllers
{
    [Route("emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IGrainFactory grains;
        public EmailsController() {
        
        }

        [HttpGet]
        [Route("{email}")]
        public IResult CheckEmail(string email)
        {
            if (!EmailHelper.IsValidEmail(email))
            {
                return Results.BadRequest();
            }

            Email parsedEmail = EmailHelper.ParseEmail(email);

            IEmailsGrain grain = grains.GetGrain<IEmailsGrain>(parsedEmail.Domain);
            var result = grain.EmailFound(parsedEmail.LocalPart);

            return Results.Ok(result);
        }

        [HttpPost]
        [Route("{emai}")]
        public IResult AddEmail(string email)
        {
            if (!EmailHelper.IsValidEmail(email))
            {
                return Results.BadRequest();
            }

            Email parsedEmail = EmailHelper.ParseEmail(email);

            IEmailsGrain grain = grains.GetGrain<IEmailsGrain>(parsedEmail.Domain);
            var result = grain.AddEmail(parsedEmail.LocalPart);

            return Results.Ok(result);
        }

    }
}
