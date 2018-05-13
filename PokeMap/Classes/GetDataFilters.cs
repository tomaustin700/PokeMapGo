using PokeMap.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokeMap.Classes
{
    public class GetDataFilters
    {
        public Bounds Bounds { get; set; }
        public string Pokemon { get; set; }
    }
}