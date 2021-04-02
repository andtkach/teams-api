﻿namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProjectCreation
    {
        [Required]
        public Guid TeamId { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
