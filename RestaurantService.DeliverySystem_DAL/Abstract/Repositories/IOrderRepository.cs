using DeliverySystem_Common.DTOs.Restaurant;
using RestaurantService.DeliverySystem_DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Abstract.Repositories
{
    public interface IOrderRepository
    {
        int? MakeOrder(CartDto cartDto);
        List<OrderDisplayDto> GetAllOrdersInProgress();
        bool AcceptOrder(AcceptOrderDto acceptOrderDto);
        List<OrderDisplayDto> GetAllAcceptedOrders();
        bool FinishOrder(int orderId);
        OrderStatus GetOrderStatus(int orderId);
        DateTime? GetOrdersAcceptanceTime(int orderId);
        List<OrderDisplayDto> GetAllFinishedOrders(string email);
        List<OrderDisplayDto> GetAllOrders();
        List<OrderDisplayDto> GetAllDelivererFinishedOrders(string email);
    }
}
