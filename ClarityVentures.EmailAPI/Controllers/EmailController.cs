using ClarityVentures.Emailer;
using Microsoft.AspNetCore.Mvc;

namespace ClarityVentures.EmailAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailProvider _emailer;

        private readonly ILogger<EmailController> _logger;

        public EmailController(ILogger<EmailController> logger, IEmailProvider emailer)
        {
            _logger = logger;
            _emailer = emailer;
        }

        [HttpGet(Name = "SendEmail")]
        public async Task<IActionResult> SendEmail(string sender, string recipient, string subject, string body)
        {
            bool success = await _emailer.SendEmail(sender, recipient, subject, body);

            if (success)
                return Ok();
            else
                return BadRequest();
        }
    }
}