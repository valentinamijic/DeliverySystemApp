using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
using DeliverySystem_Common.Enums;
using RestaurantService.DeliverySystem_DAL.Abstract.Repositories;
using RestaurantService.DeliverySystem_DAL.Abstract.Services;
using RestaurantService.DeliverySystem_DAL.Models.Enums;
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

        public List<ProductDto> GetAllProducts()
        {
            return _productRepo.GetAllProducts();
        }

        public KeyValuePair<ReturnValue, bool> RegisterNewProduct(ProductDto productDto)
        {
            if (String.IsNullOrWhiteSpace(productDto.Name)) return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);
            if (productDto.Price <= 0) return new KeyValuePair<ReturnValue, bool>(ReturnValue.INVALID_PRICE, false);

            bool exists = _productRepo.CheckIfProductExists(productDto);

            if (exists) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ALREADY_EXISTS, false);

            if (_productRepo.AddProduct(productDto)) return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true); 
            else return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false); 
        }

       
    }
}
