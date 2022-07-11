using DeliverySystem_Common.DTOs.Restaurant;
using RestaurantService.DeliverySystem_DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Abstract.Repositories
{
    public interface IProductRepository
    {
        bool CheckIfProductExists(ProductDto productDto);
        bool AddProduct(ProductDto productDto);
        List<ProductDto> GetAllProducts();
        
    }
}
