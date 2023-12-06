using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using traditional.API.Models;

namespace traditional.API.Controllers
{
    [Route("emails")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly EmailStoreDbContext _context;
        public EmailsController(EmailStoreDbContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("{email}")]
        public async Task<IResult> CheckEmail(string email)
        {
            Email parsedEmail = ParseEmail(email);
            Email? res = _context.Emails.Where(x => x.Domain == parsedEmail.Domain && x.LocalPart == parsedEmail.LocalPart).FirstOrDefault();
            return res != null ? Results.Ok("OK") : Results.NotFound("Not found");
        }

        [HttpPost]
        [Route("{email}")]
        public Task<IResult> AddEmail(string email)
        {
            Email parsedEmail = ParseEmail(email);
            Email? res = _context.Emails.Where(x => x.Domain == parsedEmail.Domain && x.LocalPart == parsedEmail.LocalPart).FirstOrDefault();
            if (res != null)
            {
                _context.Emails.Add(parsedEmail);
                return Task.FromResult(Results.Created("Created", email));
            }
            else
            {
                return Task.FromResult(Results.Conflict("Email already exists"));
            }
        }

        public static Email ParseEmail(string email)
        {
            var split = email.Split("@");
            return new Email(split[0], split[1]);
        }
    }
}
