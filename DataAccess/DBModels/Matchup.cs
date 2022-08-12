using System;
using System.Collections.Generic;

namespace DataAccess.DBModels
{
    public partial class Matchup
    {
        public Matchup()
        {
            MatchupEntries = new HashSet<MatchupEntry>();
        }

        public int Id { get; set; }
        public int? WinnerId { get; set; }
        public int MatchupRound { get; set; }
        public int TournamentId { get; set; }

        public virtual TeamDB? Winner { get; set; }
        public virtual ICollection<MatchupEntry> MatchupEntries { get; set; }
    }
}
