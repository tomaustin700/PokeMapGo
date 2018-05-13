using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PokeMap.Enums
{
    public enum SightingType
    {
        [Description("Pokémon")]
        PokeMon,
        [Description("Pokégym")]
        PokeGym,
        [Description("Lure")]
        Lure,
        [Description("Pokéstop")]
        PokeStop
    }
}