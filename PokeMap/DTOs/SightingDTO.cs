using PokeMap.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokeMap.DTOs
{
    public class SightingDTO
    {
        public int SightingId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Rarity { get; set; }
        public string Type { get; set; }
        public string AspNetUserId { get; set; }
        public int Rating { get; set; }
        public string TimeOfDay { get; set; }
        public string PokeMon { get; set; }
        public DateTime TimeAdded { get; set; }
        public string Notes { get; internal set; }

        //public Sighting()
        //{
        //    Rating = 1;

        //}
    }
}