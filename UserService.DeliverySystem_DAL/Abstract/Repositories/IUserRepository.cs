﻿using DeliverySystem_Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Repositories
{
    public interface IUserRepository
    {
        LoggedDto FindUserByEmail(string email);
        bool ChangeName(NameHandleDto nameDto);
        bool CheckIfUsernameExists(string username);
        bool ChangeUsername(UsernameHandleDto usernameDto);
        bool ChangeLastname(LastnameHandleDto lastnameDto);
        bool ChangeAddress(AddressHandleDto addressDto);
        bool ChangeDate(DateHandleDto dateDto);
        bool ChangePassword(PasswordHandleDto passwordDto, string hash);
        string FindPassword(string email);
        bool AddUsername(UsernameHandleDto usernameHandle);
        bool AddAddress(AddressHandleDto addressDto);
        DateTime? AddDate(DateHandleDto dateDto);
        List<DelivererDto> AllDeliverers();
        bool VerifyUser(VerifyDto verifyDto);
        bool RejectUser(VerifyDto verifyDto);
        bool? GetAcceptance(string email);
        bool AddPhoto(byte[]? ImageData, string? ImageMimeType, string email);
    }
}
