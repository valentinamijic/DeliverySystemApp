using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.DeliverySystem_DAL.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DelivererEmail { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public string Comment { get; set; }
        public string Address { get; set; }
        public DateTime? TimeOfMakingOrder { get; set; }
    }
}
