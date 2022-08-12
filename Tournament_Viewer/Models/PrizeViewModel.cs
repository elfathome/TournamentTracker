using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tracker_UI.Models;

public class PrizeViewModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Place number must be greater than 0 and less than 100.")]
    [Display(Name = "Place Number")]
    public int PlaceNumber { get; set; }

    [Required]
    [Display(Name = "Place Name")]
    public string? PlaceName { get; set; }

    [Range(0.00, 10000.00, ErrorMessage = "Prize must be between 0.01 and 100.00")]
    [Display(Name = "Prize Amount")]
    public decimal? PrizeAmount { get; set; }

    [Display(Name = "Prize Percentage")]
    [Range(0.00, 100.00, ErrorMessage = "Percentage must be between 0.01 and 100.00")]
    public double? PrizePercentage { get; set; }


}


