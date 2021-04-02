using System;
using System.Collections.Generic;
using System.Text;
using Teams.Services.Core.Models.Domain;

namespace Teams.Services.Core.Models.Dto
{
    public class TeamDetails
    {
        public Team Team { get; set; }

        public IEnumerable<Member> Members { get; set; }
        
        public IEnumerable<Project> Projects { get; set; }

    }
}
