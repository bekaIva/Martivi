using System;
using System.Collections.Generic;
using System.Text;

namespace Martivi.Model
{
    public class AuthenticateException : Exception
    {
        
        public AuthenticateException(string message) : base(message)
        {
        }
    }
    public class RegistrationException : Exception
    {

        public RegistrationException(string message) : base(message)
        {
        }
    }
    public class ValidityException : Exception
    {

        public ValidityException(string message) : base(message)
        {
        }
    }
}
