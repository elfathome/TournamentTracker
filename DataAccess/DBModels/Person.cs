using System;
using System.Collections.Generic;

namespace DataAccess.DBModels
{
    public partial class Person
    {
        public Person()
        {
            TeamMembers = new HashSet<TeamMember>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string? CellphoneNumber { get; set; }

        public virtual ICollection<TeamMember> TeamMembers { get; set; }
    }
}
