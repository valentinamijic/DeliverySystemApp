﻿using DeliverySystem_Common.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Repositories
{
    public interface IVerificationRepository
    {
        void AddUser(UserDto userDto);
        UserDto FindUser(string email);
        bool CheckIfUsernameExists(string username);
        bool CheckIfEmailExists(string email);
        bool CheckIfUserExists(string email);
        bool? CheckIfUserAccepted(string email);
    }
}
