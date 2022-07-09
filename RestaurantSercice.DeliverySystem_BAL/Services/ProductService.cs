using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
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

        public bool RegisterNewProduct(ProductDto productDto)
        {
            bool exists = _productRepo.CheckIfProductExists(productDto);

            if (exists) throw new Exception("Product already exists");

            if (_productRepo.AddProduct(productDto)) return true;
            else return false;
        }

        public int? MakeOrder(CartDto cartDto)
        {
            if (String.IsNullOrEmpty(cartDto.Email)) throw new Exception("Orderer email can not be empty");

            if (cartDto.CartItems.Count <= 0) throw new Exception("Cart can not be empty");

            if (String.IsNullOrEmpty(cartDto.Address)) throw new Exception("Address can not be empty");

            return _productRepo.MakeOrder(cartDto);
        }

        public List<OrderDisplayDto> GetOrders()
        {
            return _productRepo.GetAllOrdersInProgress();
        }

        public bool AcceptOrder(AcceptOrderDto acceptOrderDto)
        {
            if (String.IsNullOrEmpty(acceptOrderDto.Deliverer)) throw new Exception("No deliverer entered");

            return _productRepo.AcceptOrder(acceptOrderDto);
        }

        public DateTime? CheckIfDelivererHasOrder(string deliverer)
        {
            if (String.IsNullOrEmpty(deliverer)) throw new Exception("Deliverer can not be empty");

            List<OrderDisplayDto> ordersInProgress = _productRepo.GetAllAcceptedOrders();

            foreach (OrderDisplayDto order in ordersInProgress)
            {
                if (order.Deliverer == deliverer) return order.TimeOfAcceptingOrder;
            }
            return null;
        }

        public bool FinishOrder(int orderId)
        {
            return _productRepo.FinishOrder(orderId);
        }

        public DateTime? CheckOrderStatus(int orderId)
        {
            if (orderId < 0) throw new Exception("Invalid order id");
            OrderStatus statusOfOrder = _productRepo.GetOrderStatus(orderId);

            if (statusOfOrder == OrderStatus.ACCEPTED) return _productRepo.GetOrdersAcceptanceTime(orderId);

            return null;
        }

        public int? CheckUsersOrders(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new Exception("User can not be empty");

            List<OrderDisplayDto> activeOrders = _productRepo.GetAllOrdersInProgress();

            if (activeOrders.Count == 0) return null;
            foreach (OrderDisplayDto order in activeOrders)
            {
                if (order.Cart.Email == email) return order.Id;
            }

            return null;
        }

        public DateTime? FindOrderInDelivery(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new Exception("User can not be empty");

            List<OrderDisplayDto> ordersInDelivery = _productRepo.GetAllAcceptedOrders();

            if (ordersInDelivery.Count == 0) return null;
            foreach (OrderDisplayDto order in ordersInDelivery)
            {
                if (order.Cart.Email == email) return order.TimeOfAcceptingOrder;
            }

            return null;
        }

        public bool CheckIfOrderDelivered(int orderId)
        {
            if (orderId < 0) throw new Exception("Invalid order id");
            OrderStatus statusOfOrder = _productRepo.GetOrderStatus(orderId);

            if (statusOfOrder == OrderStatus.FINISHED) return true;

            return false;
        }

        public List<OrderDisplayDto> GetFinishedOrders(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new Exception("User can not be empty");
            return _productRepo.GetAllFinishedOrders(email);
        }

        public List<OrderDisplayDto> GetAllOrders()
        {
            return _productRepo.GetAllOrders();
        }

        public List<OrderDisplayDto> GetDelivererFinishedOrders(string email)
        {
            if (String.IsNullOrEmpty(email)) throw new Exception("User can not be empty");
            return _productRepo.GetAllDelivererFinishedOrders(email);
        }
    }
}
