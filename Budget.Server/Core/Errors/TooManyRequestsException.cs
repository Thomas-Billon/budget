using System.Diagnostics.CodeAnalysis;

namespace Budget.Server.Core.Errors
{
    public class TooManyRequestsException : Exception
    {
        public required TimeSpan? RetryAfter { get; init; }

        public TooManyRequestsException()
        {
        }

        [SetsRequiredMembers]
        public TooManyRequestsException(TimeSpan? retryAfter)
        {
            RetryAfter = retryAfter;
        }
    }
}
