namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProjectUpdate : ProjectCreation
    {
        [Required]
        public Guid Id { get; set; }
    }
}
