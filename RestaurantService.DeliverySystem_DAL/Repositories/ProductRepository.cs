using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
using RestaurantService.DeliverySystem_DAL.Abstract;
using RestaurantService.DeliverySystem_DAL.Abstract.Repositories;
using RestaurantService.DeliverySystem_DAL.Context;
using RestaurantService.DeliverySystem_DAL.Models;
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
            product.AllComponents = new List<Component>();

            product.AllComponents = components;

            _dbContext.Add(product);
            _dbContext.SaveChanges();

            return true;
        }

        public bool CheckIfProductExists(ProductDto productDto)
        {
            Product dbEntity = _dbContext.Products.Where(x => x.Name == productDto.Name).FirstOrDefault();

            if (dbEntity != null)
            {
                if (dbEntity.AllComponents.Count == productDto.Components.Count)
                {
                    return String.Join(",", dbEntity.AllComponents) == String.Join(",", productDto.Components);
                }

                return false;
            }
            return false;
        }
    }
}
