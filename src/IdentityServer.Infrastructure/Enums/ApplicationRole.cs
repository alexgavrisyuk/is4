using System.ComponentModel;

namespace IdentityServer.Infrastructure.Enums
{
    public enum ApplicationRole
    {
        [Description("User")]
        User = 0,

        [Description("Member")]
        Member = 1,

        [Description("Moderator")]
        Moderator = 2,

        [Description("Admin")]
        Admin = 3,

        [Description("Owner")]
        Owner = 4
    }
}