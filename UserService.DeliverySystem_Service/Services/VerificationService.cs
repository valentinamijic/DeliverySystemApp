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


        public TokenDto AddUser(UserDto userDto)
        {
            // First check if user with same username and email already exists
            bool usernameExists = _verificationRepo.CheckIfUsernameExists(userDto.Username);
            if (usernameExists) throw new Exception("Username with this username already exists");

            bool emailExists = _verificationRepo.CheckIfEmailExists(userDto.Email);
            if (emailExists) throw new Exception("User with this email already exists");

            HashPassword(userDto.Password, out string passwordHash);
            userDto.Password = passwordHash;

            _verificationRepo.AddUser(userDto);

            string token = GenerateToken(userDto);

            TokenDto tokenDto = new TokenDto()
            {
                Text = token
            };

            return tokenDto;

        }

        public TokenDto SignIn(UserSignInDto userDto)
        {
            UserDto user = _verificationRepo.FindUser(userDto.Email);
            if (user == null) throw new Exception("User doesn't exist");

            bool? userAccepted = _verificationRepo.CheckIfUserAccepted(userDto.Email);
            if (userAccepted == null) throw new Exception("Request is still in process!");
            if (userAccepted == false) throw new Exception("User not accepted!");

            if (user.Email == userDto.Email && user.Password != null)
            {
                if (BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password))
                {
                    string token = GenerateToken(user);

                    TokenDto tokenDto = new TokenDto()
                    {
                        Text = token
                    };

                    return tokenDto;
                }
            }
            throw new Exception("Provided email or password was not correct!");
        }

        public TokenDto AddFacebookUser(FacebookDto fb)
        {
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

                return tokenDto;
            }

            // If user doesn't exist, create it and then login
            _verificationRepo.AddUser(facebookUser);

            string token2 = GenerateToken(facebookUser);

            TokenDto tokenDto2 = new TokenDto()
            {
                Text = token2
            };

            return tokenDto2;
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
