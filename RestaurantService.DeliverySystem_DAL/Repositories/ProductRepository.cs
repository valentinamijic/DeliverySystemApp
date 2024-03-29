﻿using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
using Microsoft.EntityFrameworkCore;
using RestaurantService.DeliverySystem_DAL.Abstract;
using RestaurantService.DeliverySystem_DAL.Abstract.Repositories;
using RestaurantService.DeliverySystem_DAL.Context;
using RestaurantService.DeliverySystem_DAL.Models;
using RestaurantService.DeliverySystem_DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Models;

namespace RestaurantService.DeliverySystem_DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public ProductRepository(IMapper mapper, RestaurantDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public bool AddProduct(ProductDto productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            List<Component> components = new List<Component>();

            if (productDto.Components != null)
            {
                foreach (var component in productDto.Components)
                {
                    components.Add(_mapper.Map<Component>(component));
                }
            }
            product.Components = new List<Component>();

            product.Components = components;

            _dbContext.Add(product);
            _dbContext.SaveChanges();

            return true;
        }

        public bool CheckIfProductExists(ProductDto productDto)
        {
            List<Product> dbEntity = _dbContext.Products.Include("Components").Where(x => x.Name == productDto.Name).ToList();

            if (dbEntity != null)
            {
                bool same = false;
                bool areListsTheSame = false;

                foreach (var listItem in dbEntity)
                {
                    foreach (var newComp in productDto.Components)
                    {
                        foreach (var oldComp in listItem.Components)
                        {
                            if (newComp.Name == oldComp.Name)
                            {
                                same = true; 
                                break;
                            }
                        }

                        if (!same)
                        {
                            areListsTheSame = false;
                            same = false;
                            break;
                        }
                        else
                        {
                            areListsTheSame = true;
                            same = false;
                        }
                    }
                }

                return areListsTheSame;

            }
            return false;
        }

        public List<ProductDto> GetAllProducts()
        {
            List<Product> products = _dbContext.Products.Include("Components").ToList();

            return _mapper.Map<List<ProductDto>>(products);
        }

       
    }
}
