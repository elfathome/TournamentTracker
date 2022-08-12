using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DBModels
{
    public partial class TeamMember
    {
        [Key]
        public int Id { get; set; }

        
        public int TeamId { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual TeamDB Team { get; set; } = null!;
    }
}
