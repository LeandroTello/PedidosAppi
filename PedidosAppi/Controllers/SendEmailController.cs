using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidosAppi.Interfaces;
using Newtonsoft.Json;

namespace PedidosAppi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly ISendEmailService _sendEmailService;

        private readonly IConfiguration _config;

        public SendEmailController(ISendEmailService sendEmailService, IConfiguration config)
        {
            _sendEmailService = sendEmailService;
            _config = config;
        }

        [HttpPost]
        [Route("SendEmailRecover")]
        public async Task<IActionResult> SendEmailRecover([FromForm] string emailTo, [FromForm] string usuario)
        {
            string emailPass = _config["PasswordMail"];
            string emailFrom = _config["UserNameMail"];

            try
            {
                await _sendEmailService.SendEmailRecover(emailFrom, emailPass, emailTo, usuario);
                //return Ok("Se envio el mail correspondiente");
                return Ok( new { Success = true, message = "Se envio el mail correspondiente." });
            }

            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
