using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tracker_UI.Models;

public class TournamentViewModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Tournament Name")]
    public string? TournamentName { get; set; }

    [Range(0.00, 500.00, ErrorMessage = "ntry Fee must be between 0.01 and 100.00")]
    [Display(Name = "Entry Fee")]
    public decimal EntryFee { get; set; }

    [Display(Name = "Entered Teams")]
    public List<TeamViewModel>? EnteredTeams { get; set; }

    [Display(Name = "Prize(s)")]
    public List<PrizeViewModel>? Prizes { get; set; }

    [Display(Name = "Round(s)")]
    public List<List<MatchupViewModel>>? Rounds { get; set; }

    public string? SelectedTeams { get; set; }

    public string? SelectedPrizes { get; set; }

    public List<SelectListItem> AvailableTeamsForTourny { get; set; } = new List<SelectListItem>();

    public List<SelectListItem> AvailablePrizesForTourny { get; set; } = new List<SelectListItem>();

}
