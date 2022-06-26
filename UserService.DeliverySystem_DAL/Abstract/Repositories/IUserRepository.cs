using DeliverySystem_Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.DeliverySystem_DAL.Abstract.Repositories
{
    public interface IUserRepository
    {
        LoggedDto FindUserByEmail(string email);
    }
}
