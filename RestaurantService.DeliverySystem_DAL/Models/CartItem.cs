using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public Product product { get; set; } = new Product();
        public int Amount { get; set; }
    }
}
