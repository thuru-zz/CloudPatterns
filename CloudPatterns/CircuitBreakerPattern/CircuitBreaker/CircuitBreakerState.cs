using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CircuitBreakerPattern.CircuitBreaker
{
    public enum CircuitBreakerState
    {
        Closed,
        Open,
        PartiallyOpen
    }
}