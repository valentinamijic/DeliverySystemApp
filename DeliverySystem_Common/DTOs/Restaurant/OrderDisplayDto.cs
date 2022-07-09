using RestaurantService.DeliverySystem_DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs.Restaurant
{
    public class OrderDisplayDto
    {
        public int Id { get; set; }

        public CartDto Cart { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public int TotalAmount { get; set; }
        public string? Deliverer { get; set; }
        public DateTime? TimeOfAcceptingOrder { get; set; }
    }
}
