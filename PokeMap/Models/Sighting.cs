using PokeMap.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PokeMap.Models
{
    public class Sighting
    {
        public int SightingId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public Rarity Rarity { get; set; }
        public SightingType Type { get; set; }
        public string AspNetUserId { get; set; }
        public int Rating { get;  set; }
        public Pokemon? PokeMon { get; set; }
        public TimeOfDay TimeOfDay { get; set; }
        public string Notes { get; set; }
        public DateTime TimeAdded { get; set; }
        public Sighting()
        {
            Rating = 1;
            TimeAdded = DateTime.Now;

        }

    }
}