using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotnet_authentication.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class Auth: ControllerBase
    {
        private readonly AuthRepo _authRepo;
        public Auth(AuthRepo authRepo)
        {
            _authRepo = authRepo;
            
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<string>>> Resgister([FromBody] UserRegisterRequest user) {
            return Ok(await _authRepo.Register(
                new User{ UserName = user.UserName, Email= user.Email}, user.Password
            ));
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] UserLoginRequest user) {
            return Ok(await _authRepo.Login(user.Email, user.Password));
        }
    }
}