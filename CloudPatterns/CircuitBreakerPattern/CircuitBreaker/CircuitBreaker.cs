using CircuitBreakerPattern.CircuitBreaker.CircuitBreakerFactory;
using CircuitBreakerPattern.CircuitBreaker.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CircuitBreakerPattern.CircuitBreaker
{
    public class CircuitBreaker<T>
    {
        private static readonly object _lock = new object();

        private readonly ICircuitBreakerState _circuitBreakerstate;
        
        public CircuitBreaker(ICircuitBreakerState circuitBreakerState)
        {
            _circuitBreakerstate = circuitBreakerState;
        }

        public async Task<List<T>> ExecuteAsync(Func<string, Task<List<T>>> httpOperation, string address)
        {
            if (CanProceed)
            {
                try
                {
                    var collection = await httpOperation.Invoke(address);

                    if (_circuitBreakerstate.State == CircuitBreakerState.PartiallyOpen)
                    {
                        lock(_lock)
                        {
                            _circuitBreakerstate.CurrentContinuousSuccessCountInPartialOpen++;
                        }
                    }

                    return collection;
                }
                catch(Exception ex) when (ex.Message.Contains("500"))
                {
                    ReportFailure(ex);
                    throw _circuitBreakerstate.LastKnownException;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            throw _circuitBreakerstate.LastKnownException;
        }

        private bool CanProceed
        {
            get
            {
                switch (_circuitBreakerstate.State)
                {
                    case CircuitBreakerState.Closed: return true;

                    case CircuitBreakerState.PartiallyOpen:
                        if (_circuitBreakerstate.CurrentContinuousSuccessCountInPartialOpen >= 
                            _circuitBreakerstate.SuccessCountToBeClosedFromPartiallyOpen)
                        {
                            lock (_lock)
                            {
                                _circuitBreakerstate.State = CircuitBreakerState.Closed;
                                _circuitBreakerstate.CurrentContinuousSuccessCountInPartialOpen = 0;
                                _circuitBreakerstate.PartialOpenAllowSwitch = true;
                                return true;
                            }
                        }
                        else
                        {
                            var current = _circuitBreakerstate.PartialOpenAllowSwitch;
                            lock (_lock)
                            {
                                _circuitBreakerstate.PartialOpenAllowSwitch = !current;
                                return current;
                            }
                        }
                    case CircuitBreakerState.Open:
                        if ((DateTime.UtcNow - _circuitBreakerstate.OpendAt.Value).Seconds >= _circuitBreakerstate.OpenStateTimeout)
                        {
                            lock (_lock)
                            {
                                _circuitBreakerstate.State = CircuitBreakerState.PartiallyOpen;
                                _circuitBreakerstate.CurrentContinuousSuccessCountInPartialOpen = 0;
                                _circuitBreakerstate.OpendAt = null;

                                var current = _circuitBreakerstate.PartialOpenAllowSwitch;
                                _circuitBreakerstate.PartialOpenAllowSwitch = !current;
                                return current;
                            }
                        }
                        else return false;

                    default: throw new CircuitBreakerException("Invalid Circuit Breaker State");
                }
            }
        }

        private void ReportFailure(Exception ex)
        {
            if (_circuitBreakerstate.State == CircuitBreakerState.Closed)
            {
                lock (_lock)
                {
                    _circuitBreakerstate.CurrentFailureCountInClosed++;
                    _circuitBreakerstate.LastKnownException = ex;

                    if (_circuitBreakerstate.CurrentFailureCountInClosed == 1)
                        _circuitBreakerstate.FirstFailureReportedOn = DateTime.UtcNow;
                }

                if (_circuitBreakerstate.CurrentFailureCountInClosed >= _circuitBreakerstate.NoOfFailureToOpen)
                {
                    if ((DateTime.UtcNow - _circuitBreakerstate.FirstFailureReportedOn.Value).Seconds <= _circuitBreakerstate.FailureTimeDuration)
                    {
                        lock (_lock)
                        {
                            _circuitBreakerstate.State = CircuitBreakerState.Open;
                            _circuitBreakerstate.OpendAt = DateTime.UtcNow;
                        }
                    }

                    lock (_lock)
                    {
                        _circuitBreakerstate.CurrentFailureCountInClosed = 0;
                        _circuitBreakerstate.LastKnownException = null;
                        _circuitBreakerstate.FirstFailureReportedOn = null;
                    }

                }
            }
            if(_circuitBreakerstate.State == CircuitBreakerState.PartiallyOpen)
            {
                lock(_lock)
                {
                    _circuitBreakerstate.LastKnownException = ex;
                    _circuitBreakerstate.CurrentContinuousSuccessCountInPartialOpen = 0;
                }
            }
        }
    }
}