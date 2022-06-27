using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs
{
    public class LoggedDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool HasPassword { get; set; }
        public int UserType { get; set; }
    }
}
