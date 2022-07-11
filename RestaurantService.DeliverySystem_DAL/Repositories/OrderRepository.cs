using AutoMapper;
using DeliverySystem_Common.DTOs.Restaurant;
using Microsoft.EntityFrameworkCore;
using RestaurantService.DeliverySystem_DAL.Abstract.Repositories;
using RestaurantService.DeliverySystem_DAL.Context;
using RestaurantService.DeliverySystem_DAL.Models;
using RestaurantService.DeliverySystem_DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public OrderRepository(IMapper mapper, RestaurantDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public int? MakeOrder(CartDto cartDto)
        {
            Cart cart = _mapper.Map<Cart>(cartDto);

            List<CartItem> cartItems = new List<CartItem>();

            Order newOrder = new Order()
            {
                OrderStatus = OrderStatus.IN_PROGRESS
            };

            foreach (CartItem cartItem in cart.CartItems)
            {
                Product product = _dbContext.Products.Include("Components").FirstOrDefault(x => x.Id == cartItem.Product.Id);

                if (product == null) return null;

                CartItem newCartItem = new CartItem()
                {
                    Product = product,
                    Amount = cartItem.Amount,
                    Id = cartItem.Id
                };

                cartItems.Add(newCartItem);
            }

            cart.CartItems = cartItems;
            newOrder.Cart = cart;

            _dbContext.Add(newOrder);
            _dbContext.SaveChanges();

            return newOrder.Id;
        }

        public List<OrderDisplayDto> GetAllOrdersInProgress()
        {
            List<Order> ordersInProgress = _dbContext.Orders.Include("Cart").Where(x => x.OrderStatus == OrderStatus.IN_PROGRESS).ToList();
            List<OrderDisplayDto> ordersInProgressDto = _mapper.Map<List<OrderDisplayDto>>(ordersInProgress);

            return ordersInProgressDto;
        }

        public bool AcceptOrder(AcceptOrderDto acceptOrderDto)
        {
            Order orderForAccept = _dbContext.Orders.Include("Cart").FirstOrDefault(x => x.Id == acceptOrderDto.AcceptedOrder);

            if (orderForAccept == null) return false;

            orderForAccept.Deliverer = acceptOrderDto.Deliverer;
            orderForAccept.TimeOfAcceptingOrder = acceptOrderDto.TimeOfAcceptance;
            orderForAccept.OrderStatus = OrderStatus.ACCEPTED;

            _dbContext.SaveChanges();

            return true;
        }

        public List<OrderDisplayDto> GetAllAcceptedOrders()
        {
            List<Order> ordersAccepted = _dbContext.Orders.Include("Cart").Where(x => x.OrderStatus == OrderStatus.ACCEPTED).ToList();
            List<OrderDisplayDto> ordersInProgressDto = _mapper.Map<List<OrderDisplayDto>>(ordersAccepted);

            return ordersInProgressDto;
        }

        public bool FinishOrder(int orderId)
        {
            Order orderForConfirm = _dbContext.Orders.Include("Cart").FirstOrDefault(x => x.Id == orderId);

            if (orderForConfirm == null) return false;

            orderForConfirm.OrderStatus = OrderStatus.FINISHED;

            _dbContext.SaveChanges();

            return true;
        }

        public OrderStatus GetOrderStatus(int orderId)
        {
            OrderDisplayDto order = _mapper.Map<OrderDisplayDto>(_dbContext.Orders.Include("Cart").FirstOrDefault(x => x.Id == orderId));
            return order.OrderStatus;
        }

        public DateTime? GetOrdersAcceptanceTime(int orderId)
        {
            Order order = _dbContext.Orders.Include("Cart").FirstOrDefault(x => x.Id == orderId);

            if (order != null) return order.TimeOfAcceptingOrder;
            return null;
        }

        public List<OrderDisplayDto> GetAllFinishedOrders(string email)
        {
            List<Order> ordersFinished = _dbContext.Orders.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Product).ThenInclude(x => x.Components).Where(x => x.Cart.Email == email && x.OrderStatus == OrderStatus.FINISHED).ToList();
            List<OrderDisplayDto> ordersFinishedDto = _mapper.Map<List<OrderDisplayDto>>(ordersFinished);

            return ordersFinishedDto;
        }

        public List<OrderDisplayDto> GetAllOrders()
        {
            List<Order> allOrders = _dbContext.Orders.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Product).ThenInclude(x => x.Components).ToList();
            List<OrderDisplayDto> allOrdersDto = _mapper.Map<List<OrderDisplayDto>>(allOrders);

            return allOrdersDto;
        }

        public List<OrderDisplayDto> GetAllDelivererFinishedOrders(string email)
        {
            List<Order> ordersFinished = _dbContext.Orders.Include(x => x.Cart).ThenInclude(x => x.CartItems).ThenInclude(x => x.Product).ThenInclude(x => x.Components).Where(x => x.Deliverer == email && x.OrderStatus == OrderStatus.FINISHED).ToList();
            List<OrderDisplayDto> ordersFinishedDto = _mapper.Map<List<OrderDisplayDto>>(ordersFinished);

            return ordersFinishedDto;
        }
    }
}
