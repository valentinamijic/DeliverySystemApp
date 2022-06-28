using DeliverySystem_Common.DTOs.User;
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

        [Route("changeName")]
        [HttpPut]
        public ActionResult <bool> ChangeName(NameHandleDto nameHandle)
        {
            return _userService.HandleNameChange(nameHandle);
        }

        [Route("changeUsername")]
        [HttpPut]
        public ActionResult<bool> ChangeUsername(UsernameHandleDto usernameHandle)
        {
            return _userService.HandleUsernameChange(usernameHandle);
        }

        [Route("changeLastname")]
        [HttpPut]
        public ActionResult<bool> ChangeLastname(LastnameHandleDto lastnameHandle)
        {
            return _userService.HandleLastnameChange(lastnameHandle);
        }

        [Route("changeAddress")]
        [HttpPut]
        public ActionResult<bool> ChangeAddress(AddressHandleDto addressHandle)
        {
            return _userService.HandleAddressChange(addressHandle);
        }

        [Route("changeDate")]
        [HttpPut]
        public ActionResult<bool> ChangeDate(DateHandleDto dateHandle)
        {
            return _userService.HandleDateChange(dateHandle);
        }

        [Route("changePassword")]
        [HttpPut]
        public ActionResult<bool> ChangePassword(PasswordHandleDto passHandle)
        {
            return _userService.HandlePasswordChange(passHandle);
        }

        [Route("addUsername")]
        [HttpPost]
        public ActionResult<bool> AddUsername(UsernameHandleDto usernameHandle)
        {
            return _userService.HandleUsernameAdd(usernameHandle);
        }

        [Route("addAddress")]
        [HttpPost]
        public ActionResult<bool> AddAddress(AddressHandleDto addressHandle)
        {
            return _userService.HandleAddressAdd(addressHandle);
        }

        [Route("addDate")]
        [HttpPost]
        public ActionResult<DateTime?> AddDate(DateHandleDto dateHandle)
        {
            return _userService.HandleDateAdd(dateHandle);
        }

        [Route("deliverers")]
        [HttpGet]
        public ActionResult<List<DelivererDto>> GetDeliverers()
        {
            return _userService.FindDeliverers();
        }

        [Route("verify")]
        [HttpPut]
        public ActionResult<bool> Verify(VerifyDto verifyDto)
        {
            return _userService.VerifyUser(verifyDto);
        }

        [Route("reject")]
        [HttpPut]
        public ActionResult<bool> Reject(VerifyDto verifyDto)
        {
            return _userService.RejectUser(verifyDto);
        }
    }
}
