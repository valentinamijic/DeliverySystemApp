using DeliverySystem_Common.DTOs.User;
using DeliverySystem_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Services
{
    public interface IUserService
    {
        KeyValuePair<ReturnValue, LoggedDto> FindUser(string email);
        KeyValuePair<ReturnValue, bool> HandleNameChange(NameHandleDto nameDto);
        KeyValuePair<ReturnValue, bool> HandleUsernameChange(UsernameHandleDto usernameDto);
        KeyValuePair<ReturnValue, bool> HandleLastnameChange(LastnameHandleDto lastnameDto);
        KeyValuePair<ReturnValue, bool> HandleAddressChange(AddressHandleDto addressDto);
        KeyValuePair<ReturnValue, bool> HandleDateChange(DateHandleDto dateDto);
        KeyValuePair<ReturnValue, bool> HandlePasswordChange(PasswordHandleDto passwordDto);
        KeyValuePair<ReturnValue, bool> HandleUsernameAdd(UsernameHandleDto usernameHandle);
        KeyValuePair<ReturnValue, bool> HandleAddressAdd(AddressHandleDto addressHandle);
        KeyValuePair<ReturnValue, DateTime?> HandleDateAdd(DateHandleDto dateDto);
        KeyValuePair<ReturnValue, bool> HandlePhotoUpload(byte[]? ImageData, string? ImageMimeType, string email);

        List<DelivererDto> FindDeliverers();
        KeyValuePair<ReturnValue, bool> VerifyUser(VerifyDto verifyDto);
        KeyValuePair<ReturnValue, bool> RejectUser(VerifyDto verifyDto);
    }
}
