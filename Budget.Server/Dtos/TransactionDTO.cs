using Budget.Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Budget.Server.Dtos
{
    public class TransactionDTO
    {
        public class GetAll
        {
            public class Response : ResponseBase
            {
                public class Item
                {
                    public required int Id { get; set; }
                    public required TransactionType Type { get; set; }
                    public required double Amount { get; set; }
                    public required DateOnly? Date { get; set; }
                    public required PaymentMethod PaymentMethod { get; set; }
                    public required string Comment { get; set; }
                }

                public required IReadOnlyList<Item> Transactions { get; set; }
            }
        }

        public class Get
        {
            public class Response : ResponseBase
            {
                public required int Id { get; set; }
                public required TransactionType Type { get; set; }
                public required double Amount { get; set; }
                public required DateOnly? Date { get; set; }
                public required PaymentMethod PaymentMethod { get; set; }
                public required string Comment { get; set; }
            }
        }

        public class Create
        {
            public class Command
            {
                [Required]
                public TransactionType Type { get; init; } = TransactionType.None;

                [Required]
                public double Amount { get; init; } = 0;

                public DateOnly? Date { get; init; }

                [Required]
                public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;

                [Required(AllowEmptyStrings = true)]
                public string Comment { get; init; } = string.Empty;
            }

            public class Response: ResponseBase
            {
                public required int Id { get; set; }
                public required TransactionType Type { get; set; }
                public required double Amount { get; set; }
                public required DateOnly? Date { get; set; }
                public required PaymentMethod PaymentMethod { get; set; }
                public required string Comment { get; set; }
            }
        }

        public class Update
        {
            public class Command
            {
                [Required]
                public TransactionType Type { get; init; } = TransactionType.None;

                [Required]
                public double Amount { get; init; } = 0;

                public DateOnly? Date { get; init; }

                [Required]
                public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;

                [Required(AllowEmptyStrings = true)]
                public string Comment { get; init; } = string.Empty;
            }

            public class Response : ResponseBase
            {
                public required TransactionType Type { get; set; }
                public required double Amount { get; set; }
                public required DateOnly? Date { get; set; }
                public required PaymentMethod PaymentMethod { get; set; }
                public required string Comment { get; set; }
            }
        }

        public class UpdateType
        {
            public class Command
            {
                [Required]
                public TransactionType Type { get; init; } = TransactionType.None;
            }

            public class Response : ResponseBase
            {
                public required TransactionType Type { get; set; }
            }
        }

        public class UpdateAmount
        {
            public class Command
            {
                [Required]
                public double Amount { get; init; } = 0;
            }

            public class Response : ResponseBase
            {
                public required double Amount { get; set; }
            }
        }

        public class UpdateDate
        {
            public class Command
            {
                public DateOnly? Date { get; init; }
            }

            public class Response : ResponseBase
            {
                public required DateOnly? Date { get; set; }
            }
        }

        public class UpdatePaymentMethod
        {
            public class Command
            {
                [Required]
                public PaymentMethod PaymentMethod { get; init; } = PaymentMethod.None;
            }

            public class Response : ResponseBase
            {
                public required PaymentMethod PaymentMethod { get; set; }
            }
        }

        public class UpdateComment
        {
            public class Command
            {
                [Required(AllowEmptyStrings = true)]
                public string Comment { get; init; } = string.Empty;
            }

            public class Response : ResponseBase
            {
                public required string Comment { get; set; }
            }
        }
    }
}