using AutoMapper;
using DeliverySystem_Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Abstract.Repositories;
using UserService.DeliverySystem_DAL.Context;
using UserService.DeliverySystem_DAL.Models;

namespace UserService.DeliverySystem_DAL.Repositories
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly IMapper _mapper;
        private readonly UserDbContext _dbContext;

        public VerificationRepository(IMapper mapper, UserDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public UserDto AddUser(UserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);

            _dbContext.Add(user);
            _dbContext.SaveChanges();

            return _mapper.Map<UserDto>(user);
        }

        public UserDto FindUser(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();
            return _mapper.Map<UserDto>(dbEntity);
        }

        public bool CheckIfEmailExists(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if (dbEntity == null) return false;
            return true;    
        }

        public bool CheckIfUsernameExists(string username)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Username == username).FirstOrDefault();

            if (dbEntity == null) return false;
            return true;
        }

        public bool CheckIfUserExists(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if (dbEntity == null) return false;
            return true;
        }

        public bool CheckIfUserAccepted(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if (dbEntity != null)
            {
                if (dbEntity.Accepted) return true;
                return false;
            }

            return false;
        }


    }
}
