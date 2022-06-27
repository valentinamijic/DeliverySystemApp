using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs
{
    public class DelivererDto
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public bool? Accepted { get; set; }
    }
}
