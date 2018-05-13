using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PokeMap.Enums
{
    public enum Rarity
    {
        [Description("Common")]
        Common,

        [Description("Uncommon")]
        Uncommon,

        [Description("Rare")]
        Rare,

        [Description("Super Rare")]
        SuperRare,

        [Description("Unknown")]
        Unknown
    }
}