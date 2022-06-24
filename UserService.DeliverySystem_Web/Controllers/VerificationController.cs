using DeliverySystem_Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using UserService.DeliverySystem_DAL.Abstract.Services;

namespace UserService.DeliverySystem_Web.Controllers
{
    [Route("api/verification")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;
        private readonly ILogger<VerificationController> _logger;

        public VerificationController(IVerificationService verificationService, ILogger<VerificationController> logger)
        {
            _verificationService = verificationService;
            _logger = logger;
        }   

        [Route("register")]
        [HttpPost]
        public ActionResult <UserDto> Register (UserDto user)
        {
            return _verificationService.AddUser(user);
        }

        [Route("login")]
        [HttpPost]
        public ActionResult <TokenDto> SignIn(UserSignInDto user)
        {
            return _verificationService.SignIn(user);
        }
    }
}
