using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokeMap.Enums
{
    [Flags]
    public enum TimeOfDay
    {
        Morning = 1,
        Afternoon = 2,
        Night = 4
    }
}