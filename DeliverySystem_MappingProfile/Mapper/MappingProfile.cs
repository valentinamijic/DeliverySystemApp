using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
using DeliverySystem_Common.DTOs.User;
using RestaurantService.DeliverySystem_DAL.Models;
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
            CreateMap<User, DelivererDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Component, ComponentDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<Order, OrderDisplayDto>().ReverseMap();
        }
    }
}
