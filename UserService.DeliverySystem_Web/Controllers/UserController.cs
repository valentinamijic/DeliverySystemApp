using DeliverySystem_Common.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.DeliverySystem_DAL.Abstract.Services;

namespace UserService.DeliverySystem_Web.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [Route("find/{email}")]
        [HttpGet]
        public ActionResult<LoggedDto> GetUser(string email)
        {
            return _userService.FindUser(email);
        }
    }
}
