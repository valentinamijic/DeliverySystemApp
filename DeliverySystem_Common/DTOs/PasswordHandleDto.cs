using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.DTOs
{
    public class PasswordHandleDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}
