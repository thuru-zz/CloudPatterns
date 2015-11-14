using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CircuitBreakerPattern.CircuitBreaker
{
    public interface ICircuitBreakerState
    {
        // numbe of attempts after which the break state goes from Closed to Open
        int NoOfFailureToOpen { get;}
        // time duration in seconds to reset the No of Failure Attempts to Open
        int FailureTimeDuration { get; }
        // current state of the breaker
        CircuitBreakerState State { get; set; }
        // first failure reported on in the Closed state
        DateTime? FirstFailureReportedOn { get; set; }
        // time when circuit breaker reached Open state
        DateTime? OpendAt { get; set; }
        // last known exception
        Exception LastKnownException { get; set; }
        // how long the breaker should be in Open, once this elapsed the breaker goes to PartiallyOpen state
        int OpenStateTimeout { get; }
        // number of successful requests needed to go to Closed state from PartiallyOpen
        int SuccessCountToBeClosedFromPartiallyOpen { get; }

        int CurrentFailureCountInClosed { get; set; }
        int CurrentContinuousSuccessCountInPartialOpen { get; set; }
        bool PartialOpenAllowSwitch { get; set; }
    }
}
