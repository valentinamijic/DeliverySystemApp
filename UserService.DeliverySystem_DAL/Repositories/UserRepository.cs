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
    public class UserRepository : IUserRepository
    { 
        private readonly IMapper _mapper;
        private readonly UserDbContext _dbContext;

        public UserRepository(IMapper mapper, UserDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public LoggedDto FindUserByEmail(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();
            return _mapper.Map<LoggedDto>(dbEntity);
        }
    }
}
