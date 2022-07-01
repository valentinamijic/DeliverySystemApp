using DeliverySystem_Common.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.DeliverySystem_DAL.Abstract.Services;

namespace RestaurantService.DeliverySystem_Web.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("registerProduct")]
        [HttpPost]
        public ActionResult<bool> AddProduct(ProductDto product)
        {
            return _productService.RegisterNewProduct(product);
        }

        [Route("products")]
        [HttpGet]
        public ActionResult<List<ProductDto>> GetProducts()
        {
            return _productService.GetAllProducts();
        }
    }
}
