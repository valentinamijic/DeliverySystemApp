using DeliverySystem_Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Services
{
    public interface IUserService
    {
        LoggedDto FindUser(string email);
        bool HandleNameChange(NameHandleDto nameDto);
        bool HandleUsernameChange(UsernameHandleDto usernameDto);
        bool HandleLastnameChange(LastnameHandleDto lastnameDto);
        bool HandleAddressChange(AddressHandleDto addressDto);
        bool HandleDateChange(DateHandleDto dateDto);
        bool HandlePasswordChange(PasswordHandleDto passwordDto);
        bool HandleUsernameAdd(UsernameHandleDto usernameHandle);
        bool HandleAddressAdd(AddressHandleDto addressHandle);
        DateTime? HandleDateAdd(DateHandleDto dateDto);
        List<DelivererDto> FindDeliverers();
        bool VerifyUser(VerifyDto verifyDto);
        bool RejectUser(VerifyDto verifyDto);
    }
}
