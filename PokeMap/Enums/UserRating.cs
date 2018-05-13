using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PokeMap.Enums
{
    public enum UserRating
    {
        [Description("Bronze")]

        Bronze,
        [Description("Silver")]

        Silver,
        [Description("Top Contributor")]

        TopContributor
    }
}