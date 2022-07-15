
using DeliverySystem_Common.DTOs.User;
using DeliverySystem_Common.Enums;
using Microsoft.AspNetCore.Mvc;
using UserService.DeliverySystem_DAL.Abstract.Services;

namespace UserService.DeliverySystem_Web.Controllers
{
    [Route("api/verification")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;

        public VerificationController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }   



        [Route("register")]
        [HttpPost]
        public ActionResult <TokenDto> Register(UserDto user)
        {
            KeyValuePair<ReturnValue, TokenDto> ret = _verificationService.AddUser(user);

            if (ret.Key == ReturnValue.EMAIL_EXISTS) return BadRequest("User with this email already exists!");
            else if (ret.Key == ReturnValue.USERNAME_EXISTS) return BadRequest("User with this username already exists!");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("All fields must be entered!");

            return Ok(ret.Value);
        }

        [Route("login")]
        [HttpPost]
        public ActionResult <TokenDto> SignIn(UserSignInDto user)
        {
            KeyValuePair<ReturnValue, TokenDto> ret = _verificationService.SignIn(user);

            if (ret.Key == ReturnValue.DOESNT_EXIST) return NotFound("User with this credentials doesn't exist!");
            else if (ret.Key == ReturnValue.REQUEST_IN_PROCESS) return Accepted("Your registration request is still processing by administrators. Check again later!");
            else if (ret.Key == ReturnValue.NOT_ACCEPTED) return BadRequest("Your registration request is declined.");
            else if (ret.Key == ReturnValue.INCORRECT_CREDENTIALS) return BadRequest("Provided email or password was not correct!");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("All fields must be entered!");

            return Ok(ret.Value);
        }

        [Route("facebookRegister")]
        [HttpPost]
        public ActionResult<TokenDto> FacebookSignUp(FacebookDto user)
        {
            KeyValuePair<ReturnValue, TokenDto> ret = _verificationService.AddFacebookUser(user);
            if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("All fields must be entered!");
            return Ok(ret.Value);
        }
    }
}
