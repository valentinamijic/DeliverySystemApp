using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.DeliverySystem_DAL.Models.Enums;

namespace UserService.DeliverySystem_DAL.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string? Username { get; set; }    
        public string? Password { get; set; }    
        public string Email { get; set; }    
        public string Name { get; set; }    
        public string Lastname { get; set; }
        public DateTime? BirthDate { get; set; } = null;
        public string? Address { get; set; }
        public UserType UserType { get; set; }
        public bool? Accepted { get; set; } = null;
        public byte[]? ImageData { get; set; }
        public string? ImageMimeType { get; set; }

    }
}
