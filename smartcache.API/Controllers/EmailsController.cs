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
                return Results.BadRequest();
            }

            Email parsedEmail = EmailHelper.ParseEmail(email);

            bool result = false;
            try
            {
                var grain = _grains.GetGrain<IEmailsGrain?>(parsedEmail.Domain);
                result = await grain.EmailFound(parsedEmail.LocalPart);
            }
            catch (NullReferenceException e)
            {
                Results.Ok(result);
            }
            

            return Results.Ok(result);
        }

        [HttpPost]
        [Route("{email}")]
        public IResult AddEmail(string email)
        {
            if (!EmailHelper.IsValidEmail(email))
            {
                return Results.BadRequest();
            }

            Email parsedEmail = EmailHelper.ParseEmail(email);

            IEmailsGrain grain = _grains.GetGrain<IEmailsGrain>(parsedEmail.Domain);
            var result = grain.AddEmail(parsedEmail.LocalPart);

            return Results.Ok(result);
        }

    }
}
