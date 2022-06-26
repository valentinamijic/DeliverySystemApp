using AutoMapper;
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
    }
}
