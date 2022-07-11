using DeliverySystem_Common.DTOs.Restaurant;
using DeliverySystem_Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Abstract.Services
{
    public interface IOrderService
    {
        KeyValuePair<ReturnValue, int?> MakeOrder(CartDto cartDto);
        List<OrderDisplayDto> GetOrders();
        KeyValuePair<ReturnValue, bool> AcceptOrder(AcceptOrderDto acceptOrderDto);
        KeyValuePair<ReturnValue, DateTime?> CheckIfDelivererHasOrder(string deliverer);
        KeyValuePair<ReturnValue, bool> FinishOrder(int orderId);
        KeyValuePair<ReturnValue, DateTime?> CheckOrderStatus(int orderId);
        KeyValuePair<ReturnValue, int?> CheckUsersOrders(string email);
        KeyValuePair<ReturnValue, DateTime?> FindOrderInDelivery(string email);
        KeyValuePair<ReturnValue, bool> CheckIfOrderDelivered(int orderId);
        List<OrderDisplayDto> GetFinishedOrders(string email);
        List<OrderDisplayDto> GetAllOrders();
        List<OrderDisplayDto> GetDelivererFinishedOrders(string email);
    }
}
