using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokeMap.Classes
{
    public class Bounds
    {
        public double bottomLeftLat { get; set; }
        public double bottomLeftLong { get; set; }
        public double topRightLat { get; set; }
        public double topRightLong { get; set; }
    }
}