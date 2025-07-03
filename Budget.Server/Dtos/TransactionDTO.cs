using Budget.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Dtos
{
    public class TransactionDTO
    {
        public class TransactionBase
        {
            public required int Id { get; set; }
            public required double Amount { get; set; }
            public required DateOnly? Date { get; set; }
            public required TransactionPaymentMethod? PaymentMethod { get; set; }
            public required string? Comment { get; set; }
        }

        public class TransactionResponseBase : ResponseBase
        {
            public required int Id { get; set; }
            public required double Amount { get; set; }
            public required DateOnly? Date { get; set; }
            public required TransactionPaymentMethod? PaymentMethod { get; set; }
            public required string? Comment { get; set; }
        }

        public class GetAll
        {
            public class Response : ResponseBase
            {
                public required IReadOnlyList<TransactionBase> Transactions { get; set; }
            }
        }

        public class Get
        {
            public class Response : TransactionResponseBase
            {
            }
        }

        public class Create
        {
            public record Command(
                [property: Required] double Amount,
                DateOnly? Date,
                TransactionPaymentMethod? PaymentMethod,
                string? Comment
            );

            public class Response : TransactionResponseBase
            {
            }
        }

        public class Update
        {
            public record Command(
                double Amount,
                DateOnly? Date,
                TransactionPaymentMethod? PaymentMethod,
                string? Comment
            );

            public class Response : ResponseBase
            {
                public required double Amount { get; set; }
                public required DateOnly? Date { get; set; }
                public required TransactionPaymentMethod? PaymentMethod { get; set; }
                public required string? Comment { get; set; }
            }
        }

        public class UpdateAmount
        {
            public record Command(double Amount);

            public class Response : ResponseBase
            {
                public required double Amount { get; set; }
            }
        }

        public class UpdateDate
        {
            public record Command(DateOnly? Date);

            public class Response : ResponseBase
            {
                public required DateOnly? Date { get; set; }
            }
        }

        public class UpdatePaymentMethod
        {
            public record Command(TransactionPaymentMethod? PaymentMethod);

            public class Response : ResponseBase
            {
                public required TransactionPaymentMethod? PaymentMethod { get; set; }
            }
        }

        public class UpdateComment
        {
            public record Command(string? Comment);

            public class Response : ResponseBase
            {
                public required string? Comment { get; set; }
            }
        }
    }
}