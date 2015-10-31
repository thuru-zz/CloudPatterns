using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheAsidePatternWebTemplate.Cache
{
    public interface ICacheable
    {
        string CacheKey { get; set; }
        double ? LifeTime { get; set; }
    }
}
