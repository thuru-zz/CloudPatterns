using CacheAsidePatternWebTemplate.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CacheAsidePatternWebTemplate.Models
{
    public class Product : ICacheable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
    }
}