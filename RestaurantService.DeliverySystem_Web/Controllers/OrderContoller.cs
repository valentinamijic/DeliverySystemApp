﻿using DeliverySystem_Common.DTOs.Restaurant;
using DeliverySystem_Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.DeliverySystem_DAL.Abstract.Services;

namespace RestaurantService.DeliverySystem_Web.Controllers
{
    [Route("api/order")]
    [ApiController]

    public class OrderContoller : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderContoller(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Route("order")]
        [Authorize(Roles = "2")]
        [HttpPost]
        public ActionResult<int?> MakeOrder(CartDto cart) 
        {
            if (String.IsNullOrEmpty(cart.StripeToken)) return BadRequest("Payment unsuccessfull.");
            KeyValuePair<ReturnValue, int?> retVal = _orderService.MakeOrder(cart);

            if (retVal.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (retVal.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("Entered fields can't be empty!");
            else if (retVal.Key == ReturnValue.NO_COMPONENTS) return BadRequest("Cart can't be empty!");

            return Ok(retVal.Value);
        }

        [Route("orders")]
        [Authorize(Roles = "3")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetAllOrdersInProgress()
        {
            return _orderService.GetOrders();
        }

        [Route("accept")]
        [Authorize(Roles = "3")]
        [HttpPut]
        public ActionResult<bool> AcceptOrder(AcceptOrderDto orderDetalis)
        {
            Thread.BeginCriticalRegion();
            KeyValuePair<ReturnValue, bool> retVal = _orderService.AcceptOrder(orderDetalis);

            if (retVal.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured.");
            else if (retVal.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("One or more errors occured.");

            Thread.EndCriticalRegion();
            return Ok(retVal.Value);
        }


        [Route("checkOrderAmount/{deliverer}")]
        [HttpGet]
        public ActionResult<DateTime?> FindDelivererActiveOrder(string deliverer)
        {
            KeyValuePair<ReturnValue, DateTime?> retVal = _orderService.CheckIfDelivererHasOrder(deliverer);

            if (retVal.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("One or more errors occured.");
            return Ok(retVal.Value);
        }

        [Route("confirm")]
        [HttpPut]
        public ActionResult<bool> MakeOrderFinished(FinishedOrderDto finishedOrder)
        {
            KeyValuePair<ReturnValue, bool> retVal =  _orderService.FinishOrder(finishedOrder.OrderId);

            if (retVal.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("One or more errors occured");
            else if (retVal.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured");

            return Ok(true);
        }

        [Route("orderStatus/{orderId}")]
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult<DateTime?> GetOrderStatus(int orderId)
        {
            KeyValuePair<ReturnValue, DateTime?> retVal = _orderService.CheckOrderStatus(orderId);

            if (retVal.Key == ReturnValue.EMPTY_FIELDS) return BadRequest("One or more errors occured");
            else if (retVal.Key == ReturnValue.ERROR_OCCURED) return BadRequest("One or more errors occured");


            return Ok(retVal.Value);
        }

        [Route("activeOrder/{email}")]
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult<int?> GetUserActiveOrder(string email)
        {
            KeyValuePair<ReturnValue, int?> retVal = _orderService.CheckUsersOrders(email);
            return Ok(retVal.Value);
        }

        [Route("orderInDelivery/{email}")]
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult<DateTime?> GetUserOrderInDelivery(string email)
        {
            KeyValuePair<ReturnValue, DateTime?> retVal = _orderService.FindOrderInDelivery(email);
            return Ok(retVal.Value);
        }

        [Route("orderDelivered/{orderId}")]
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult<bool> CheckIfOrderDelivered(int orderId)
        {
            KeyValuePair<ReturnValue, bool> retVal = _orderService.CheckIfOrderDelivered(orderId);
            return Ok(retVal.Value);
        }

        [Route("finishedOrders/{email}")]
        [Authorize(Roles = "2")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetFinishedOrders(string email)
        {
            return Ok(_orderService.GetFinishedOrders(email));
        }

        [Route("allOrders")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetAllOrders()
        {
            return Ok(_orderService.GetAllOrders());
        }

        [Route("delivererOrders/{email}")]
        [Authorize(Roles = "3")]
        [HttpGet]
        public ActionResult<List<OrderDisplayDto>> GetDelivererFinishedOrders(string email)
        {
            return Ok(_orderService.GetDelivererFinishedOrders(email));
        }
    }
}
