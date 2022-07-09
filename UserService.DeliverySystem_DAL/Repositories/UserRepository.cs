using AutoMapper;
using DeliverySystem_Common.DTOs.User;
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

            if (dbEntity != null)
            {
                LoggedDto logged = _mapper.Map<LoggedDto>(dbEntity);

                if (dbEntity.Password != null) logged.HasPassword = true;
                else logged.HasPassword = false;

                if (dbEntity.ImageData != null && dbEntity.ImageMimeType != null)
                {
                    logged.Image = "data:" + dbEntity.ImageMimeType + ";base64," + @Convert.ToBase64String(dbEntity.ImageData);
                }

                return logged;
            } return null;
        }

        public bool ChangeName(NameHandleDto nameDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == nameDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Name = nameDto.Name;
            _dbContext.SaveChanges();
            return true;
        }

        public bool CheckIfUsernameExists(string username)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Username == username).FirstOrDefault();

            if (dbEntity == null) return false;
            return true;
        }

        public bool ChangeUsername(UsernameHandleDto nameDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == nameDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Username = nameDto.Username;
            _dbContext.SaveChanges();
            return true;
        }

        public bool ChangeLastname(LastnameHandleDto lastnameDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == lastnameDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Lastname = lastnameDto.Lastname;
            _dbContext.SaveChanges();
            return true;
        }

        public bool ChangeAddress(AddressHandleDto addressDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == addressDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Address = addressDto.Address;
            _dbContext.SaveChanges();
            return true;
        }

        public bool ChangeDate(DateHandleDto dateDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == dateDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.BirthDate = dateDto.Date;
            _dbContext.SaveChanges();
            return true;
        }

        public bool ChangePassword(PasswordHandleDto passwordDto, string hash)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == passwordDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Password = hash;
            _dbContext.SaveChanges();
            return true;
        }

        public string FindPassword(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if (dbEntity != null) return dbEntity.Password;
            
            return null;
        }

        public bool AddUsername(UsernameHandleDto usernameHandle)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == usernameHandle.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Username = usernameHandle.Username;
            _dbContext.SaveChanges();
            return true;
        }

        public bool AddAddress(AddressHandleDto addressHandle)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == addressHandle.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Address = addressHandle.Address;
            _dbContext.SaveChanges();
            return true;
        }

        public DateTime? AddDate(DateHandleDto dateDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == dateDto.Email).FirstOrDefault();

            if (dbEntity == null) return null;

            dbEntity.BirthDate = dateDto.Date;
            _dbContext.SaveChanges();
            return dbEntity.BirthDate;
        }

        public List<DelivererDto> AllDeliverers()
        {
            return _mapper.Map<List<DelivererDto>>(_dbContext.Users.Where(x => x.UserType == Models.Enums.UserType.DELIVERER).ToList());
        }

        public bool VerifyUser(VerifyDto verifyDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == verifyDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Accepted = true;
            _dbContext.SaveChanges();
            return true;
        }

        public bool RejectUser(VerifyDto verifyDto)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == verifyDto.Email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.Accepted = false;
            _dbContext.SaveChanges();
            return true;
        }

        public bool? GetAcceptance(string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if (dbEntity != null) return dbEntity.Accepted;

            return null;
        }

        public bool AddPhoto(byte[]? ImageData, string? ImageMimeType, string email)
        {
            User dbEntity = _dbContext.Users.Where(x => x.Email == email).FirstOrDefault();

            if (dbEntity == null) return false;

            dbEntity.ImageMimeType = ImageMimeType;
            dbEntity.ImageData = ImageData;
            _dbContext.SaveChanges();

            return true;

        }
    }
}
