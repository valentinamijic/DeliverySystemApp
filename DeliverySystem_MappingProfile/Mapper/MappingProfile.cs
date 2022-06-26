using AutoMapper;
using DeliverySystem_Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Models;

namespace DeliverySystem_MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, LoggedDto>().ReverseMap();
        }
    }
}
