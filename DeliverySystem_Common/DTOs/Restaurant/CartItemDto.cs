using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs.Restaurant
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public ProductDto product { get; set; } = new ProductDto();
        public int Amount { get; set; }
    }
}
