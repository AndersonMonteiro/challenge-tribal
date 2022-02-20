using System;
using System.Globalization;

namespace Tribal.CreditLine.Domain.Exceptions
{
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException() : base() { }

        public TooManyRequestsException(string message) : base(message) { }

        public TooManyRequestsException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
