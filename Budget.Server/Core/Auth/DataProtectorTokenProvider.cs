using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Budget.Server.Core.Auth
{
    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public const string PROVIDER_NAME = "EmailConfirmation";

        public EmailConfirmationTokenProviderOptions()
        {
            Name = PROVIDER_NAME;
        }
    }

    public class ResetPasswordTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        public const string PROVIDER_NAME = "ResetPassword";

        public ResetPasswordTokenProviderOptions()
        {
            Name = PROVIDER_NAME;
        }
    }

    public class DataProtectorTokenProvider<TUser, TOptions> : DataProtectorTokenProvider<TUser>
        where TUser : IdentityUser
        where TOptions : DataProtectionTokenProviderOptions
    {
        public DataProtectorTokenProvider(
            IDataProtectionProvider dataProtectionProvider,
            IOptions<TOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
        }
    }
}
