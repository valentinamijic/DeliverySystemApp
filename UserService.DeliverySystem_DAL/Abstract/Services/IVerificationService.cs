using DeliverySystem_Common.DTOs.User;
using DeliverySystem_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Services
{
    public interface IVerificationService
    {
        KeyValuePair<ReturnValue, TokenDto> AddUser(UserDto userDto);
        KeyValuePair<ReturnValue, TokenDto> SignIn(UserSignInDto userDto);
        KeyValuePair<ReturnValue, TokenDto> AddFacebookUser(FacebookDto fb);
    }
}
