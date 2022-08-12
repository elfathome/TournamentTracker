using System;
using System.ComponentModel.DataAnnotations;

namespace Tracker_UI.Models
{
    public class MatchupViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Matchup Round")]
        public int MatchupRound { get; set; }

        [Display(Name = "Winner")]
        public TeamViewModel Winner { get; set; }

        [Display(Name = "Matchup Entries")]
        public List<MatchupEntryViewModel>? Entries { get; set; }

        public int? WinnerId { get; set; }

        public int TournamentId { get; set; }

        [Display(Name = "Tournament")]
        public TournamentViewModel? Tournament { get; set; }
    }
}

