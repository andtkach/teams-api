namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TeamCreation
    {
        [Required]
        public string DisplayName { get; set; }
    }
}
