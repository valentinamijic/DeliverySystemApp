using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs.Restaurant
{
    public class CartDto
    {
        public string Email { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public string Comment { get; set; }
        public string Address { get; set; }
        public int TotalAmount { get; set; }
        public DateTime TimeOfMakingOrder { get; set; }
    }
}
