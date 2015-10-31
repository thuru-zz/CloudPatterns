using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheAsidePatternWebTemplate.Cache
{
    public class CacheProvderException : Exception
    {
        public CacheProvderException(string message)
            : base(message)
        {
        }
    }
}