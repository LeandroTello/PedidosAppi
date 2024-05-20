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
                return Ok("Se envio el mail correspondiente");
            }

            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("ValidateUser")]
        public async Task<IActionResult> ValidateUser([FromForm] string user, [FromForm] string recoveryCode)
        {
            try
            {
                if(await _sendEmailService.ValidateUser(user, recoveryCode)) 
                {
                    return Ok(new { success = true, message = "Usuario validado." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Usuario invalido." });
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex.Message });
            }
        }
    }
}
