using AutoMapper;
using DeliverySystem_Common.DTOs.User;
using DeliverySystem_Common.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Abstract.Repositories;
using UserService.DeliverySystem_DAL.Abstract.Services;

namespace UserService.DeliverySystem_BAL.Services
{
    public class VerificationService : IVerificationService
    {
        private readonly IMapper _mapper;
        private readonly IVerificationRepository _verificationRepo;
        private readonly IConfiguration _configuration;

        public VerificationService(IMapper mapper, IVerificationRepository verificationRepo, 
                                   IConfiguration configuration)
        {
            _mapper = mapper;
            _verificationRepo = verificationRepo;
            _configuration = configuration; 
        }


        public KeyValuePair<ReturnValue, TokenDto> AddUser(UserDto userDto)
        {
            if (String.IsNullOrWhiteSpace(userDto.Address) || String.IsNullOrWhiteSpace(userDto.Name) 
                || String.IsNullOrWhiteSpace(userDto.Email) || String.IsNullOrWhiteSpace(userDto.Lastname)
                || String.IsNullOrWhiteSpace(userDto.Username) || String.IsNullOrWhiteSpace(userDto.Password)
                || userDto.BirthDate == new DateTime() || userDto.UserType < 0) 
                
                return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.EMPTY_FIELDS, null);

            // First check if user with same username and email already exists
            bool usernameExists = _verificationRepo.CheckIfUsernameExists(userDto.Username);
            if (usernameExists) return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.USERNAME_EXISTS, null);

            bool emailExists = _verificationRepo.CheckIfEmailExists(userDto.Email);
            if (emailExists) return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.EMAIL_EXISTS, null);

            HashPassword(userDto.Password, out string passwordHash);
            userDto.Password = passwordHash;

            _verificationRepo.AddUser(userDto);

            string token = GenerateToken(userDto);

            TokenDto tokenDto = new TokenDto()
            {
                Text = token
            };

            return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.OK, tokenDto);
        }

        public KeyValuePair<ReturnValue, TokenDto> SignIn(UserSignInDto userDto)
        {
            if (String.IsNullOrWhiteSpace(userDto.Email) || String.IsNullOrWhiteSpace(userDto.Password))
                return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.EMPTY_FIELDS, null);

            UserDto user = _verificationRepo.FindUser(userDto.Email);
            if (user == null) return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.DOESNT_EXIST, null);

            bool? userAccepted = _verificationRepo.CheckIfUserAccepted(userDto.Email);
            if (userAccepted == null) return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.REQUEST_IN_PROCESS, null);
            if (userAccepted == false) return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.NOT_ACCEPTED, null);

            if (user.Email == userDto.Email && user.Password != null)
            {
                if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                {
                    string token = GenerateToken(user);

                    TokenDto tokenDto = new TokenDto()
                    {
                        Text = token
                    };

                    return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.OK, tokenDto);
                }
            }
            return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.INCORRECT_CREDENTIALS, null);
        }

        public KeyValuePair<ReturnValue, TokenDto> AddFacebookUser(FacebookDto fb)
        {
            if (String.IsNullOrWhiteSpace(fb.Email) || String.IsNullOrWhiteSpace(fb.Name) || string.IsNullOrWhiteSpace(fb.Lastname))
                return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.EMPTY_FIELDS, null);
            // First check if user exists
            bool userExists = _verificationRepo.CheckIfUserExists(fb.Email);

            UserDto facebookUser = new UserDto()
            {
                Email = fb.Email,
                Name = fb.Name,
                Lastname = fb.Lastname,
                UserType = 2,
            };

            if (userExists)
            {
                string token = GenerateToken(facebookUser);

                TokenDto tokenDto = new TokenDto()
                {
                    Text = token
                };

                return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.OK, tokenDto);
            }

            // If user doesn't exist, create it and then login
            _verificationRepo.AddUser(facebookUser);

            string token2 = GenerateToken(facebookUser);

            TokenDto tokenDto2 = new TokenDto()
            {
                Text = token2
            };

            return new KeyValuePair<ReturnValue, TokenDto>(ReturnValue.OK, tokenDto2);
        }



        private string GenerateToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, user.UserType.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:4200",
                claims: claims, 
                expires: DateTime.Now.AddMinutes(60), 
                signingCredentials: signinCredentials 
            );
            return  new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        private void HashPassword(string password, out string passwordHash)
        {
            passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
