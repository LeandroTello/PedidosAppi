using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PedidosAppi.Interfaces;

namespace PedidosAppi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly IUserManagerService _userManagerService;

        private readonly IConfiguration _config;

        public UserManagerController (IUserManagerService userManagerService, IConfiguration config)
        {
            _userManagerService = userManagerService;
            _config = config;
        }

        [HttpPost]
        [Route("ValidateUser")]
        public async Task<IActionResult> ValidateUser([FromForm] string user, [FromForm] string recoveryCode)
        {
            try
            {
                if (await _userManagerService.ValidateUser(user, recoveryCode))
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

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] string user, [FromForm] string password)
        {
            try
            {
                await _userManagerService.ChangePassword(user, password);
                return Ok(new { success = true, message = "Contraseña reestablecida." });
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex.Message });
            }
        }
    }
}
