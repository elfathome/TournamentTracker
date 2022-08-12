using System;
using System.ComponentModel.DataAnnotations;

namespace Tracker_UI.Models
{
    public class TournamentPrizeViewModel
    {
        [Key]
        public int Id { get; set; }

        public int TournamentId { get; set; }

        public TournamentViewModel? Tournament { get; set; }

        public int PrizeId { get; set; }

        public PrizeViewModel? Prize { get; set; }
    }
}

