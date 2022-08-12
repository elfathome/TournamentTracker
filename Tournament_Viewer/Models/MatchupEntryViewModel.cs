using System.ComponentModel.DataAnnotations;

namespace Tracker_UI.Models
{
    public class MatchupEntryViewModel
    {
        [Key]
        public int Id { get; set; }

        public decimal Score { get; set; }

        public int MatchupId { get; set; }

        public int? ParentMatchupId { get; set; }

        public int? TeamCompetingId { get; set; }

        [Display(Name = "Matchup")]
        public MatchupViewModel? Matchup { get; set; }

        [Display(Name = "Previous Matchup")]
        public MatchupViewModel? ParentMatchup { get; set; }

        public TeamViewModel? TeamCompeting { get; set; }

    }
}