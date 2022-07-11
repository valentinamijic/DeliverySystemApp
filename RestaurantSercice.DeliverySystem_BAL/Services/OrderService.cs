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
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepo;

        public OrderService(IMapper mapper, IOrderRepository orderRepo)
        {
            _mapper = mapper;
            _orderRepo = orderRepo;
        }

        public KeyValuePair<ReturnValue, int?> MakeOrder(CartDto cartDto)
        {
            if (String.IsNullOrWhiteSpace(cartDto.Email) || String.IsNullOrWhiteSpace(cartDto.Address)) return new KeyValuePair<ReturnValue, int?>(ReturnValue.EMPTY_FIELDS, null);
            if (cartDto.CartItems.Count <= 0) return new KeyValuePair<ReturnValue, int?>(ReturnValue.NO_COMPONENTS, null);


            int? orderId = _orderRepo.MakeOrder(cartDto);

            if (orderId == null) return new KeyValuePair<ReturnValue, int?>(ReturnValue.ERROR_OCCURED, null);

            return new KeyValuePair<ReturnValue, int?>(ReturnValue.OK, orderId);
        }

        public List<OrderDisplayDto> GetOrders()
        {
            return _orderRepo.GetAllOrdersInProgress();
        }

        public KeyValuePair<ReturnValue, bool> AcceptOrder(AcceptOrderDto acceptOrderDto)
        {
            if (String.IsNullOrEmpty(acceptOrderDto.Deliverer) || acceptOrderDto.AcceptedOrder == null
                || acceptOrderDto.TimeOfAcceptance == null) return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            bool ret = _orderRepo.AcceptOrder(acceptOrderDto);
            if (!ret) return new KeyValuePair<ReturnValue, bool>(ReturnValue.ERROR_OCCURED, false);

            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, DateTime?> CheckIfDelivererHasOrder(string deliverer)
        {
            if (String.IsNullOrEmpty(deliverer)) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.EMPTY_FIELDS, null);

            List<OrderDisplayDto> ordersInProgress = _orderRepo.GetAllAcceptedOrders();

            foreach (OrderDisplayDto order in ordersInProgress)
            {
                if (order.Deliverer == deliverer) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, order.TimeOfAcceptingOrder);
            }

            return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, null);
        }

        public KeyValuePair<ReturnValue, bool> FinishOrder(int orderId)
        {
            if (orderId < 0) return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);

            bool ret = _orderRepo.FinishOrder(orderId);

            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);
        }

        public KeyValuePair<ReturnValue, DateTime?> CheckOrderStatus(int orderId)
        {
            if (orderId < 0) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.EMPTY_FIELDS, null);
            OrderStatus statusOfOrder = _orderRepo.GetOrderStatus(orderId);

            if (statusOfOrder == OrderStatus.ACCEPTED) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, _orderRepo.GetOrdersAcceptanceTime(orderId)); 

            return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, null);
        }

        public KeyValuePair<ReturnValue, int?> CheckUsersOrders(string email)
        {
            if (String.IsNullOrEmpty(email)) return new KeyValuePair<ReturnValue, int?>(ReturnValue.EMPTY_FIELDS, null);

            List<OrderDisplayDto> activeOrders = _orderRepo.GetAllOrdersInProgress();

            if (activeOrders.Count == 0) return  new KeyValuePair<ReturnValue, int?>(ReturnValue.OK, null); 
            foreach (OrderDisplayDto order in activeOrders)
            {
                if (order.Cart.Email == email) return new KeyValuePair<ReturnValue, int?>(ReturnValue.OK, order.Id);
            }

            return new KeyValuePair<ReturnValue, int?>(ReturnValue.OK, null);
        }

        public KeyValuePair<ReturnValue, DateTime?> FindOrderInDelivery(string email)
        {
            if (String.IsNullOrEmpty(email)) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.EMPTY_FIELDS, null);

            List<OrderDisplayDto> ordersInDelivery = _orderRepo.GetAllAcceptedOrders();

            if (ordersInDelivery.Count == 0) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, null);
            foreach (OrderDisplayDto order in ordersInDelivery)
            {
                if (order.Cart.Email == email) return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, order.TimeOfAcceptingOrder); 
            }

            return new KeyValuePair<ReturnValue, DateTime?>(ReturnValue.OK, null); 
        }

        public KeyValuePair<ReturnValue, bool> CheckIfOrderDelivered(int orderId)
        {
            if (orderId < 0) return new KeyValuePair<ReturnValue, bool>(ReturnValue.EMPTY_FIELDS, false);
            OrderStatus statusOfOrder = _orderRepo.GetOrderStatus(orderId);

            if (statusOfOrder == OrderStatus.FINISHED) return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, true);

            return new KeyValuePair<ReturnValue, bool>(ReturnValue.OK, false);
        }

        public List<OrderDisplayDto> GetFinishedOrders(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new Exception("User can not be empty");
            return _orderRepo.GetAllFinishedOrders(email);
        }

        public List<OrderDisplayDto> GetAllOrders()
        {
            return _orderRepo.GetAllOrders();
        }

        public List<OrderDisplayDto> GetDelivererFinishedOrders(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new Exception("User can not be empty");
            return _orderRepo.GetAllDelivererFinishedOrders(email);
        }

    }
}
