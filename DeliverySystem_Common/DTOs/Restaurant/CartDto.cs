using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs.Restaurant
{
    public class CartDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DelivererEmail { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public string Comment { get; set; }
        public string Address { get; set; }
        public DateTime TimeOfMakingOrder { get; set; }
    }
}
