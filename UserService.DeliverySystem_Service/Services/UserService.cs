﻿using AutoMapper;
using DeliverySystem_Common.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Abstract.Repositories;
using UserService.DeliverySystem_DAL.Abstract.Services;

namespace UserService.DeliverySystem_BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;

        public UserService(IMapper mapper, IUserRepository userRepo)
        {
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public LoggedDto FindUser(string email)
        {
            if(!String.IsNullOrWhiteSpace(email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                return loggedUser;
            }
            throw new Exception("Email is empty");
        }


        public bool HandleNameChange(NameHandleDto nameDto)
        {
            if (!String.IsNullOrWhiteSpace(nameDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(nameDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(nameDto.Name))
                {
                    return _userRepo.ChangeName(nameDto);
                }
                throw new Exception("New name is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandleUsernameChange(UsernameHandleDto usernameDto)
        {
            if (!String.IsNullOrWhiteSpace(usernameDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(usernameDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(usernameDto.Username))
                {
                    bool exists = _userRepo.CheckIfUsernameExists(usernameDto.Username);
                    if (exists) return false;

                    return _userRepo.ChangeUsername(usernameDto);
                }
                throw new Exception("New username is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandleLastnameChange(LastnameHandleDto lastnameDto)
        {
            if (!String.IsNullOrWhiteSpace(lastnameDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(lastnameDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(lastnameDto.Lastname))
                {
                    return _userRepo.ChangeLastname(lastnameDto);
                }
                throw new Exception("New lastname is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandleAddressChange(AddressHandleDto addressDto)
        {
            if (!String.IsNullOrWhiteSpace(addressDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(addressDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(addressDto.Address))
                {
                    return _userRepo.ChangeAddress(addressDto);
                }
                throw new Exception("New address is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandleDateChange(DateHandleDto dateDto)
        {
            if (!String.IsNullOrWhiteSpace(dateDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(dateDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                DateTime startDate = new DateTime();

                if (DateTime.Compare(dateDto.Date, startDate) != 0)
                {
                    return _userRepo.ChangeDate(dateDto);
                }
                throw new Exception("New date is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandlePasswordChange(PasswordHandleDto passwordDto)
        {
            if (!String.IsNullOrWhiteSpace(passwordDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(passwordDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(passwordDto.Password))
                {
                    string password = _userRepo.FindPassword(passwordDto.Email);

                    if (password != null)
                    {
                        if (BCrypt.Net.BCrypt.Verify(passwordDto.OldPassword, password))
                        {

                            HashPassword(passwordDto.Password, out string passwordHash);

                            return _userRepo.ChangePassword(passwordDto, passwordHash);
                        }
                        return false;
                    }
                }
                throw new Exception("New password is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandleUsernameAdd(UsernameHandleDto usernameHandle)
        {
            if (!String.IsNullOrWhiteSpace(usernameHandle.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(usernameHandle.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(usernameHandle.Username))
                {
                    bool exists = _userRepo.CheckIfUsernameExists(usernameHandle.Username);

                    if (exists) return false;

                    return _userRepo.AddUsername(usernameHandle);
                }
                throw new Exception("New username is empty");
            }
            throw new Exception("Email is empty");
        }

        public bool HandleAddressAdd(AddressHandleDto addressHandle)
        {
            if (!String.IsNullOrWhiteSpace(addressHandle.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(addressHandle.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                if (!String.IsNullOrEmpty(addressHandle.Address))
                {
                    return _userRepo.AddAddress(addressHandle);
                }
                throw new Exception("New address is empty");
            }
            throw new Exception("Email is empty");
        }

        public DateTime? HandleDateAdd(DateHandleDto dateDto)
        {
            if (!String.IsNullOrWhiteSpace(dateDto.Email))
            {
                LoggedDto loggedUser = _userRepo.FindUserByEmail(dateDto.Email);
                if (loggedUser == null) throw new Exception("User doesn't exist");

                DateTime startDate = new DateTime();

                if (DateTime.Compare(dateDto.Date, startDate) != 0)
                {
                    return _userRepo.AddDate(dateDto);
                }

                throw new Exception("New date is empty");
            }
            throw new Exception("Email is empty");
        }

        private void HashPassword(string password, out string passwordHash)
        {
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }


    }
}
