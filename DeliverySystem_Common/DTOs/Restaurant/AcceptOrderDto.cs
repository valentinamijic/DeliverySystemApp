using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs.Restaurant
{
    public class AcceptOrderDto
    {
        public string Deliverer { get; set; }
        public DateTime? TimeOfAcceptance { get; set; }
        public int? AcceptedOrder { get; set; }

    }
}
