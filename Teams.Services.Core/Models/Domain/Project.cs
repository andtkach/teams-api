namespace Teams.Services.Core.Models.Domain
{
    using System;
    using Teams.Services.Core.Models.Common;

    public class Project : ModelBase
    {
        public Guid TeamId { get; set; }

        public string DisplayName { get; set; }

        public int Priority { get; set; }

        public DateTime? StartDate { get; set; }

        public string Location { get; set; }
    }
}
