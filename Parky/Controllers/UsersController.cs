using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;

namespace ParkyApi.Controllers
{
    [Route("api/v{version:int}/Users")]
    [ApiController]
    [Authorize]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _user;
        public UsersController(IUserRepository _user)
        {
            this._user = _user;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDto model)
        {
            var user = _user.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest();
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserDto model)
        {
            bool unique = _user.IsUserUnique(model.Username);
            if (!unique) 
                return BadRequest(new { message = "User Already Exists!" });
            var user =  _user.Register(model.Username.ToString(), model.Password);
            if (user == null)
            {
                return BadRequest(new {message = "Error while Registering" });
            }
            return Ok(user);
        }
    }
}
