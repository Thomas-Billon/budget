namespace Budget.Server.Core.Errors
{
    public static class ErrorCodes
    {
        public static class Field {
            public const string Required = "error.field.required";
        }

        public static class Email
        {
            public const string TooLong = "error.email.too_long";
            public const string InvalidFormat = "error.email.invalid_format";
        }

        public static class Name
        {
            public const string TooLong = "error.name.too_long";
        }

        public static class Password
        {
            public const string TooShort = "error.password.too_short";
            public const string TooLong = "error.password.too_long";
            public const string MissingUppercase = "error.password.missing_uppercase";
            public const string MissingLowercase = "error.password.missing_lowercase";
            public const string MissingDigit = "error.password.missing_digit";
            public const string MissingSpecial = "error.password.missing_special";
        }

        public static class Auth
        {
            public const string InvalidCredentials = "error.auth.invalid_credentials";
            public const string RegistrationFailed = "error.auth.registration_failed";
            public const string TokenExpired = "error.auth.token_expired";
            public const string ResetPasswordFailed = "error.auth.reset_password_failed";
            public const string EmailConfirmationFailed = "error.auth.email_confirmation_failed";
        }

        public static class Transaction
        {
            public const string NotFound = "error.transaction.not_found";
            public const string CannotCreate = "error.transaction.cannot_create";
            public const string CannotUpdate = "error.transaction.cannot_update";
            public const string CannotPatch = "error.transaction.cannot_patch";
            public const string CannotDelete = "error.transaction.cannot_delete";
        }

        public static class Category
        {
            public const string NotFound = "error.category.not_found";
            public const string CannotCreate = "error.category.cannot_create";
            public const string CannotUpdate = "error.category.cannot_update";
            public const string CannotPatch = "error.category.cannot_patch";
            public const string CannotDelete = "error.category.cannot_delete";
        }

        public static class Account
        {
            public const string NotFound = "error.account.not_found";
            public const string CannotCreate = "error.account.cannot_create";
            public const string CannotUpdate = "error.account.cannot_update";
            public const string CannotPatch = "error.account.cannot_patch";
            public const string CannotDelete = "error.account.cannot_delete";
        }

        public static class Server
        {
            public const string Internal = "error.server.internal";
            public const string NotImplemented = "error.server.not_implemented";
            public const string Unauthorized = "error.server.unauthorized";
            public const string TooManyRequests = "error.server.too_many_requests";
        }
    }
}
