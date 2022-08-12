using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tracker_UI.Models
{
    public class TeamViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Team Name")]
        public string TeamName { get; set; } = "";
    }
}

