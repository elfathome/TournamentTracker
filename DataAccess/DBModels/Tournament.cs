using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DBModels
{
    public partial class Tournament
    {
        public Tournament()
        {
            TournamentEntries = new HashSet<TournamentEntry>();
            TournamentPrizes = new HashSet<TournamentPrize>();
            Rounds = new List<List<Matchup>>();
        }

        public int Id { get; set; }
        public string TournamentName { get; set; } = null!;
        public decimal EntryFee { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<TournamentEntry> TournamentEntries { get; set; }
        public virtual ICollection<TournamentPrize> TournamentPrizes { get; set; }

        [NotMapped]
        public virtual List<List<Matchup>> Rounds { get; set; }
    }
}
