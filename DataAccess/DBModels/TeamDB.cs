using System;
using System.Collections.Generic;

namespace DataAccess.DBModels
{
    public partial class TeamDB
    {
        public TeamDB()
        {
            MatchupEntries = new HashSet<MatchupEntry>();
            Matchups = new HashSet<Matchup>();
            TeamMembers = new HashSet<TeamMember>();
            TournamentEntries = new HashSet<TournamentEntry>();
        }

        public int Id { get; set; }
        public string TeamName { get; set; } = null!;

        public virtual ICollection<MatchupEntry> MatchupEntries { get; set; }
        public virtual ICollection<Matchup> Matchups { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; }
        public virtual ICollection<TournamentEntry> TournamentEntries { get; set; }
    }
}
