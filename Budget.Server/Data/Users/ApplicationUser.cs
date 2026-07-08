using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace Budget.Server.Data.Users
{
    public class ApplicationUser : IdentityUser
    {
        // INFO: Workaround for the IdentityUser.Email being nullable
        [AllowNull]
        [ProtectedPersonalData]
        public override required string Email
        {
            get => base.Email ?? string.Empty;
            set => base.Email = value;
        }

        [ProtectedPersonalData]
        public string? FirstName { get; set; }

        [ProtectedPersonalData]
        public string? LastName { get; set; }
    }
}
