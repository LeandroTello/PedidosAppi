using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PedidosAppi.Interfaces;

namespace PedidosAppi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly ISendEmailService _sendEmailService;

        private readonly IConfiguration _config;

        public SendEmailController (ISendEmailService sendEmailService, IConfiguration config)
        {
            _sendEmailService = sendEmailService;
            _config = config;
        }

        public async Task<IActionResult> SendEmailRecover([FromForm] string emailTo, [FromForm] string emailBody, [FromForm] string codRecuperacion)
        {
           string emailPass = _config["PasswordMail"];
           string emailFrom = _config["UserNameMail"];

           try
            {
                await _sendEmailService.SendEmailRecover(emailFrom, emailTo, emailBody, emailPass, codRecuperacion);
                return Ok("Se envio el mail correspondiente");
            }
           
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
