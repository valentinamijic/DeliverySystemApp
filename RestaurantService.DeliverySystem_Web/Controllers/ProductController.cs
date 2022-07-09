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

        [Route("order")]
        [HttpPost]
        public ActionResult<int?> MakeOrder(CartDto cart)
        {
            return _productService.MakeOrder(cart);
        }

        [Route("orders")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetAllOrdersInProgress()
        {
            return _productService.GetOrders();
        }

        [Route("accept")]
        [HttpPut]
        public ActionResult<bool> AcceptOrder(AcceptOrderDto orderDetalis)
        {
            return _productService.AcceptOrder(orderDetalis);
        }

        [Route("checkOrderAmount/{deliverer}")]
        [HttpGet]
        public ActionResult<DateTime?> FindDelivererActiveOrder(string deliverer)
        {
            return _productService.CheckIfDelivererHasOrder(deliverer);
        }

        [Route("confirm")]
        [HttpPut]
        public ActionResult<bool> MakeOrderFinished(FinishedOrderDto finishedOrder)
        {
            return _productService.FinishOrder(finishedOrder.OrderId);
        }

        [Route("orderStatus/{orderId}")]
        [HttpGet]
        public ActionResult<DateTime?> GetOrderStatus(int orderId)
        {
            return _productService.CheckOrderStatus(orderId);
        }

        [Route("activeOrder/{email}")]
        [HttpGet]
        public ActionResult<int?> GetUserActiveOrder(string email)
        {
            return _productService.CheckUsersOrders(email);
        }

        [Route("orderInDelivery/{email}")]
        [HttpGet]
        public ActionResult<DateTime?> GetUserOrderInDelivery(string email)
        {
            return _productService.FindOrderInDelivery(email);
        }

        [Route("orderDelivered/{orderId}")]
        [HttpGet]
        public ActionResult<bool> CheckIfOrderDelivered(int orderId)
        {
            return _productService.CheckIfOrderDelivered(orderId);
        }

        [Route("finishedOrders/{email}")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetFinishedOrders(string email)
        {
            return _productService.GetFinishedOrders(email);
        }

        [Route("allOrders")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetAllOrders()
        {
            return _productService.GetAllOrders();
        }

        [Route("delivererOrders/{email}")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetDelivererFinishedOrders(string email)
        {
            return _productService.GetDelivererFinishedOrders(email);
        }
    }
}
