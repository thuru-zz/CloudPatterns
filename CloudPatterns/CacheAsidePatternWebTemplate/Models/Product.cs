using CacheAsidePatternWebTemplate.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheAsidePatternWebTemplate.Models
{
    public class Product : ICacheable
    {
        public string CacheKey
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double? LifeTime
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}