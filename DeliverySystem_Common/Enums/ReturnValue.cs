using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliverySystem_Common.Enums
{
    public enum ReturnValue
    {
        OK,
        USERNAME_EXISTS,
        EMAIL_EXISTS,
        EMPTY_FIELDS,
        REQUEST_IN_PROCESS,
        NOT_ACCEPTED,
        INCORRECT_CREDENTIALS,
        DOESNT_EXIST,
        INVALID_PASSWORD,
        ERROR_OCCURED
    }
}
