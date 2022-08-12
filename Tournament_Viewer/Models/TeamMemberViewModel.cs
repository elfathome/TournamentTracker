using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Tracker_UI.Models
{
    public class TeamMemberViewModel
    {
        [Key]
        public int Id { get; set; }


        public int TeamId { get; set; }


        public int PersonId { get; set; }

        public TeamViewModel? Team { get; set; }

        public PersonViewModel? Person { get; set; }
    }
}

