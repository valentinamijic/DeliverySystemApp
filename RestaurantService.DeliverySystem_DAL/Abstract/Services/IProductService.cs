using DeliverySystem_Common.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Abstract.Services
{
    public interface IProductService
    {
        bool RegisterNewProduct(ProductDto productDto);
        List<ProductDto> GetAllProducts();
    }
}
