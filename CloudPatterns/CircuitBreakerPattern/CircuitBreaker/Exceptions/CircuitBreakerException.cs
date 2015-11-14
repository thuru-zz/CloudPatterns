using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CircuitBreakerPattern.CircuitBreaker.Exceptions
{
    public class CircuitBreakerException : Exception
    {
        public CircuitBreakerException(string message)
            : base(message)
        {

        }
    }
}