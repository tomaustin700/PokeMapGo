using PokeMap.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokeMap.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public string AspNetUserId { get; set; }
        public int SightingId { get; set; }
        public VoteAction Action { get; set; }
    }
}