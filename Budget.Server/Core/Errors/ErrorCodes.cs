namespace Budget.Server.Core.Errors
{
    public static class ErrorCodes
    {
        public static class Field {
            public const string Required = "error.field.required";
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

        public static class Server
        {
            public const string Internal = "error.server.internal";
            public const string NotImplemented = "error.server.not_implemented";
            public const string TooManyRequests = "error.server.too_many_requests";
        }
    }
}
