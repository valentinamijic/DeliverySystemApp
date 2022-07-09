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
        int? MakeOrder(CartDto cartDto);
        List<OrderDisplayDto> GetOrders();
        bool AcceptOrder(AcceptOrderDto acceptOrderDto);
        DateTime? CheckIfDelivererHasOrder(string deliverer);
        bool FinishOrder(int orderId);
        DateTime? CheckOrderStatus(int orderId);
        int? CheckUsersOrders(string email);
        DateTime? FindOrderInDelivery(string email);
        bool CheckIfOrderDelivered(int orderId);
        List<OrderDisplayDto> GetFinishedOrders(string email);
        List<OrderDisplayDto> GetAllOrders();
        List<OrderDisplayDto> GetDelivererFinishedOrders(string email);
    }
}
