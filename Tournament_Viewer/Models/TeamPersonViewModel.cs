using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tracker_UI.Models
{
    public class TeamPersonViewModel
    {
        public TeamViewModel? Team { get; set; }

        [BindProperty]
        public List<SelectListItem>? AvailablePeopleForTeam { get; set; }

        [Display(Name = "Team Members")]
        [Required]
        public string SelectedPeople { get; set; }

        [BindProperty]
        public PersonViewModel Person { get; set; } = new PersonViewModel();
    }
}

