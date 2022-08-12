using System;
using System.Collections.Generic;

namespace DataAccess.DBModels
{
    public partial class MatchupEntry
    {

        public int Id { get; set; }
        public int MatchupId { get; set; }
        public int? ParentMatchupId { get; set; }
        public int? TeamCompetingId { get; set; }
        public double? Score { get; set; }

        public virtual Matchup Matchup { get; set; } = null;
        public virtual Matchup? ParentMatchup { get; set; }
        public virtual TeamDB? TeamCompeting { get; set; }
    }
}
