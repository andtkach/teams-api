namespace Teams.Services.Core.Models.MethodActions.Teams
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MemberUpdate : MemberCreation
    {
        [Required]
        public Guid Id { get; set; }
    }
}
