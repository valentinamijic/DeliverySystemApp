using DeliverySystem_Common.DTOs.Restaurant;
using DeliverySystem_Common.Enums;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult<bool> AddProduct(ProductDto product)
        {
            KeyValuePair<ReturnValue, bool> retVal = _productService.RegisterNewProduct(product);

            if (retVal.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (retVal.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Entered fields can't be empty!");
            else if (retVal.Key == ReturnValue.INVALID_PRICE) return BadRequest("Price must be positive value!");
            else if (retVal.Key == ReturnValue.ALREADY_EXISTS) return BadRequest("Product like this already exists!");

            return Ok(retVal.Value);
        }

        [Route("products")]
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult<List<ProductDto>> GetProducts()
        {
            return Ok(_productService.GetAllProducts());
        }

       
    }
}
