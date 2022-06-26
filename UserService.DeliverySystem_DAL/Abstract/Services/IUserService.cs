using DeliverySystem_Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Services
{
    public interface IUserService
    {
        LoggedDto FindUser(string email);
    }
}
