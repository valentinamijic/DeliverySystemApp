using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
using RestaurantService.DeliverySystem_DAL.Abstract.Repositories;
using RestaurantService.DeliverySystem_DAL.Abstract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_BAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepo;

        public ProductService(IMapper mapper, IProductRepository productRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
        }

        public bool RegisterNewProduct(ProductDto productDto)
        {
            bool exists = _productRepo.CheckIfProductExists(productDto);

            if (exists) throw new Exception("Product already exists");

            if (_productRepo.AddProduct(productDto)) return true;
            else return false;
        }
    }
}
