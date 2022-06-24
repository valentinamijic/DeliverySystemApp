using AutoMapper;
using DeliverySystem_Common.DTOs;
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


        public UserDto AddUser(UserDto userDto)
        {
            // First check if user with same username and email already exists
            bool usernameExists = _verificationRepo.CheckIfUsernameExists(userDto.Username);
            if (usernameExists) throw new Exception("Username already exists");

            bool emailExists = _verificationRepo.CheckIfEmailExists(userDto.Email);
            if (usernameExists) throw new Exception("Email already exists");

            HashPassword(userDto.Password, out string passwordHash);
            userDto.Password = passwordHash;

            return _verificationRepo.AddUser(userDto); 
        }

        public TokenDto SignIn(UserSignInDto userDto)
        {
            UserDto user = _verificationRepo.FindUser(userDto.Email);
            if (user == null) throw new Exception("User doesn't exist");

            //bool userAccepted = _verificationRepo.CheckIfUserAccepted(userDto.Email);
            //if (!userAccepted) throw new Exception("Not accepted");

            if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
            {
                string token = GenerateToken(user);

                TokenDto tokenDto = new TokenDto()
                {
                    Text = token
                };

                return tokenDto;
            }

            return null;
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
                //issuer: "http://localhost:44302"
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
