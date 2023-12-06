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
        private readonly IGrainFactory _grains;
        public EmailsController(IGrainFactory grains) {
            _grains = grains;
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IResult> CheckEmail(string email)
        {
            if (!EmailHelper.IsValidEmail(email))
            {
                return Results.BadRequest("The entered email is not valid");
            }

            Email parsedEmail = EmailHelper.ParseEmail(email);

            var grain = _grains.GetGrain<IEmailsGrain>(parsedEmail.Domain);
            bool result = await grain.EmailFound(parsedEmail.LocalPart);
            return result ? Results.Ok("OK") : Results.NotFound("Not found");
        }

        [HttpPost]
        [Route("{email}")]
        public async Task<IResult> AddEmail(string email)
        {
            if (!EmailHelper.IsValidEmail(email))
            {
                return Results.BadRequest("The entered email is not valid");
            }

            Email parsedEmail = EmailHelper.ParseEmail(email);

            IEmailsGrain grain = _grains.GetGrain<IEmailsGrain>(parsedEmail.Domain);
            bool result = await grain.AddEmail(parsedEmail.LocalPart);
            return result ? Results.Created("Created", email) : Results.Conflict("Email already exists");

        }

    }
}
