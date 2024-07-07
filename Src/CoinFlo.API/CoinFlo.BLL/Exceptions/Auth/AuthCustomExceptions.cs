using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinFlo.BLL.Exceptions.Auth
{
    public class AuthCustomExceptions
    {
        public class EmailAlreadyInUseException : Exception
        {
            public EmailAlreadyInUseException() : base("Email is already in use!") { }
        }

        public class PhoneAlreadyInUseException : Exception
        {
            public PhoneAlreadyInUseException() : base("Phone is already in use!") { }
        }
    }
}
