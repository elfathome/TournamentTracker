using System;
using System.ComponentModel.DataAnnotations;
using DataAccess.DBModels;

namespace Tracker_UI.Models
{
    public class PersonViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Display(Name = "Cellphone")]
        public string? CellphoneNumber { get; set; }

        public string FullName {
            get
            {
                return $"{ FirstName } { LastName }";
            }

            set { }
        }
       
    }
}

