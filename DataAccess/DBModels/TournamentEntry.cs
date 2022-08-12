using System;
using System.Collections.Generic;

namespace DataAccess.DBModels
{
    public partial class TournamentEntry
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int TeamId { get; set; }

        public virtual TeamDB Team { get; set; } = null!;
        public virtual Tournament Tournament { get; set; } = null!;
    }
}
