using System;
using System.ComponentModel.DataAnnotations;

namespace Tracker_UI.Models
{
    public class TournamentEntriesViewModel
    {
        [Key]
        public int Id { get; set; }

        public int TournamentId { get; set; }

        public TournamentViewModel? Tournament { get; set; }

        public int TeamId { get; set; }

        public TeamViewModel? Team { get; set; }
    }
}

