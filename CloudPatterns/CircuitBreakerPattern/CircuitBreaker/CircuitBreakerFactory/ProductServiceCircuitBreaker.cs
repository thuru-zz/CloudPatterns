using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace CircuitBreakerPattern.CircuitBreaker.CircuitBreakerFactory
{
    public class ProductServiceCircuitBreaker : ICircuitBreakerState
    {
        public int FailureTimeDuration { get; }
        public Exception LastKnownException { get; set; }
        public DateTime? FirstFailureReportedOn { get; set; }
        public DateTime? OpendAt { get; set; }
        public int NoOfFailureToOpen { get; }
        public int OpenStateTimeout { get; }
        public CircuitBreakerState State { get; set; }
        public int SuccessCountToBeClosedFromPartiallyOpen { get; }
       
        public int CurrentFailureCountInClosed { get; set; }
        public int CurrentContinuousSuccessCountInPartialOpen { get; set; }
        public bool PartialOpenAllowSwitch { get; set; }

       
        public ProductServiceCircuitBreaker()
        {
            State = CircuitBreakerState.Closed;
            
            // breaker will go Open if 8 failures occur in 20 seconds
            NoOfFailureToOpen = 8;
            FailureTimeDuration = 20;

            // breaker will stay in Open state for 15 seconds once enters the state
            OpenStateTimeout = 15;

            // after 6 continous successful attempts in Partial Open state, breaker will enter the Closed state
            SuccessCountToBeClosedFromPartiallyOpen = 6;

            // first attempt in Partial Open will be allowed
            PartialOpenAllowSwitch = true;
        }
    }
}