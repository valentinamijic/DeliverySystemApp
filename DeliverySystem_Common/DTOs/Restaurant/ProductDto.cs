using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs.Restaurant
{
    public class ProductDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public List<ComponentDto> Components { get; set; }
        public int Amount { get; set; } = 0;
    }
}
