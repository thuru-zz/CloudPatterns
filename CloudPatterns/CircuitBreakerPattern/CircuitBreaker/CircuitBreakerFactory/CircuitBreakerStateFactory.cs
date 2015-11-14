using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CircuitBreakerPattern.CircuitBreaker.CircuitBreakerFactory
{
    public static class CircuitBreakerStateFactory
    {
        private readonly static ProductServiceCircuitBreaker _productServiceCBState;

        static CircuitBreakerStateFactory()
        {
            _productServiceCBState = new ProductServiceCircuitBreaker();
        }

        public static ICircuitBreakerState ProductServiceCircuitBreakerState
        {
            get
            {
                return _productServiceCBState;
            }
        }
    }
}