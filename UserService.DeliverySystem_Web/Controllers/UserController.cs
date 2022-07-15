using DeliverySystem_Common.DTOs.User;
using DeliverySystem_Common.Enums;
using Microsoft.AspNetCore.Authorization;
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
            KeyValuePair<ReturnValue, LoggedDto> ret = _userService.FindUser(email);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            return Ok(ret.Value);
        }

        [Route("changeName")]
        [HttpPut]
        public ActionResult <bool> ChangeName(NameHandleDto nameHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleNameChange(nameHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Name field can't be empty.");

            return Ok(ret.Value);
        }

        [Route("changeUsername")]
        [HttpPut]
        public ActionResult<bool> ChangeUsername(UsernameHandleDto usernameHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleUsernameChange(usernameHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.USERNAME_EXISTS) return BadRequest("Entered username already exists. Pick another!");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Username field can't be empty.");

            return Ok(ret.Value);
        }

        [Route("changeLastname")]
        [HttpPut]
        public ActionResult<bool> ChangeLastname(LastnameHandleDto lastnameHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleLastnameChange(lastnameHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Lastname field can't be empty.");

            return Ok(ret.Value);
        }

        [Route("changeAddress")]
        [HttpPut]
        public ActionResult<bool> ChangeAddress(AddressHandleDto addressHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleAddressChange(addressHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Address field can't be empty.");

            return Ok(ret.Value);
        }

        [Route("changeDate")]
        [HttpPut]
        public ActionResult<bool> ChangeDate(DateHandleDto dateHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleDateChange(dateHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Date field can't be empty.");

            return Ok(ret.Value);
        }

        [Route("changePassword")]
        [HttpPut]
        public ActionResult<bool> ChangePassword(PasswordHandleDto passHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandlePasswordChange(passHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Password field can't be empty.");
            else if (ret.Key == ReturnValue.INVALID_PASSWORD) return BadRequest("Invalid password was entered. Try again!");

            return Ok(ret.Value);
        }

        [Route("addUsername")]
        [HttpPost]
        public ActionResult<bool> AddUsername(UsernameHandleDto usernameHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleUsernameAdd(usernameHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Username can't be empty.");
            else if (ret.Key == ReturnValue.USERNAME_EXISTS) return BadRequest("Username already exists. Pick another!");

            return Ok(ret.Value);
        }

        [Route("addAddress")]
        [HttpPost]
        public ActionResult<bool> AddAddress(AddressHandleDto addressHandle)
        {
            KeyValuePair<ReturnValue, bool> ret = _userService.HandleAddressAdd(addressHandle);
            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Address can't be empty.");

            return Ok(ret.Value);
        }

        [Route("addDate")]
        [HttpPost]
        public ActionResult<DateTime?> AddDate(DateHandleDto dateHandle)
        {
            KeyValuePair<ReturnValue, DateTime?> ret = _userService.HandleDateAdd(dateHandle);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Date time can't be empty.");

            return Ok(ret.Value);
        }


        [Route("addPhoto")]
        [HttpPost]
        public ActionResult<bool> AddPhoto()
        {
            var image = Request.Form.Files[0];
            var email = Request.Form.Files[1];

            var imageName = Path.GetFileName(image.FileName);
            var emailName = Path.GetFileName(email.FileName);

            var path = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\UserService.DeliverySystem_DAL\\Photos\\" + imageName;

            using (var stream = new FileStream(path, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            var imageMimeType = image.ContentType;
            var imageData = System.IO.File.ReadAllBytes(path);

            KeyValuePair<ReturnValue, bool> ret = _userService.HandlePhotoUpload(imageData, imageMimeType, emailName);
            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (ret.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Photo can't be empty.");

            return Ok(true);

        }

        [Route("deliverers")]
        [Authorize(Roles = "1")]
        [HttpGet]
        public ActionResult<List<DelivererDto>> GetDeliverers()
        {
            return Ok(_userService.FindDeliverers());
        }

        [Route("verify")]
        [Authorize(Roles = "1")]
        [HttpPut]
        public ActionResult<bool> Verify(VerifyDto verifyDto)
        {
            Thread.BeginCriticalRegion();
            KeyValuePair<ReturnValue, bool> ret = _userService.VerifyUser(verifyDto);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");

            Thread.EndCriticalRegion();
            return Ok(true);
        }

        [Route("reject")]
        [Authorize(Roles = "1")]
        [HttpPut]
        public ActionResult<bool> Reject(VerifyDto verifyDto)
        {
            Thread.BeginCriticalRegion();
            KeyValuePair<ReturnValue, bool> ret = _userService.RejectUser(verifyDto);

            if (ret.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");

            Thread.EndCriticalRegion();
            return Ok(true);
        }

    }
}
