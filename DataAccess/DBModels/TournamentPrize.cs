using System;
using System.Collections.Generic;

namespace DataAccess.DBModels
{
    public partial class TournamentPrize
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int PrizeId { get; set; }

        public virtual Prize Prize { get; set; } = null!;
        public virtual Tournament Tournament { get; set; } = null!;
    }
}
